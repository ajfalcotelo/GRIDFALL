using System.Collections.Generic;
using UnityEngine;

public class TargetSelectionState : BaseState
{
    private readonly PlayerUnitController unitController;
    private readonly InputController inputController;
    private readonly NodeHighlighter nodeHighlighter;
    private readonly MovementPreviewSystem movementPreviewSystem;
    private List<PathNode> plannedPath;
    private PathNode prevHoverNode;
    private bool hasHovered;

    public TargetSelectionState(
        StateMachine stateMachine,
        IUnitRoot unit,
        PlayerUnitController unitController,
        InputController inputController,
        NodeHighlighter nodeHighlighter,
        MovementPreviewSystem movementPreviewSystem
    )
        : base(stateMachine, unit)
    {
        this.unitController = unitController;
        this.inputController = inputController;
        this.nodeHighlighter = nodeHighlighter;
        this.movementPreviewSystem = movementPreviewSystem;
    }

    public override void Enter()
    {
        inputController.EnableSelectionInputs();
        inputController.Click += OnClick;
        inputController.Hover += OnHover;
        inputController.Cancel += OnCancelSelection;
        nodeHighlighter.HighlightNodes(unitController.SelectedAction.GetReachableNodes());
    }

    public override void Exit()
    {
        inputController.Click -= OnClick;
        inputController.Hover -= OnHover;
        inputController.Cancel -= OnCancelSelection;
        inputController.DisableSelectionInputs();
    }

    private void OnClick(Vector3 mousePos)
    {
        PathNode selectedNode = GridManager.Instance.PathfindLayer.GetNode(mousePos);

        ActionContext context = new()
        {
            Actor = unit,
            TargetUnit = GridManager.Instance.OccupancyLayer.GetNode(selectedNode.Position),
            TargetNode = selectedNode,
        };

        if (!unitController.SelectedAction.CanRun(context) || !IsNodeValid(selectedNode))
            return;

        nodeHighlighter.ClearSetTiles();
        movementPreviewSystem.Clear();
        unitController.SelectedTargetNode = selectedNode;
        unitController.SelectedPath = plannedPath;
        ChangeState(unitController.ActionExecutionState);
    }

    private void OnHover(Vector2 mousePosition)
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        if (unitController.SelectedAction == unitController.MoveAction)
        {
            MoveHover(mouseWorldPosition);
            return;
        }

        PathNode hoveredNode = GridManager.Instance.PathfindLayer.GetNode(mouseWorldPosition);
        if (IsNodeValid(hoveredNode))
        {
            nodeHighlighter.HighlightHover(mouseWorldPosition);
        }
        else
        {
            nodeHighlighter.ClearHover();
        }
    }

    private void MoveHover(Vector3 mouseWorldPosition)
    {
        PathNode hoveredNode = GridManager.Instance.PathfindLayer.GetNode(mouseWorldPosition);
        if (hoveredNode == null || !hoveredNode.IsWalkable || !IsNodeValid(hoveredNode))
        {
            movementPreviewSystem.Clear();
            nodeHighlighter.ClearHover();
            prevHoverNode = null;
            hasHovered = false;
            return;
        }

        if (!hasHovered)
        {
            prevHoverNode = hoveredNode;
            PreviewMove(mouseWorldPosition, hoveredNode);
            hasHovered = true;
        }

        if (hoveredNode != prevHoverNode)
        {
            PreviewMove(mouseWorldPosition, hoveredNode);
            prevHoverNode = hoveredNode;
        }
    }

    private bool IsNodeValid(PathNode selectedNode)
    {
        var reachableNodes = unitController.SelectedAction.GetReachableNodes();
        return reachableNodes.Exists(node => node == selectedNode);
    }

    private void PreviewMove(Vector3 mouseWorldPosition, PathNode hoveredNode)
    {
        List<PathNode> path = Pathfinding.FindPath(unit.CurrentNode.Position, hoveredNode.Position);
        movementPreviewSystem.RenderPreview(unit, path);
        nodeHighlighter.HighlightHover(mouseWorldPosition);
        plannedPath = path;
    }

    private void OnCancelSelection()
    {
        ChangeState(unitController.ActionSelectionState);
        nodeHighlighter.ClearSetTiles();
    }
}
