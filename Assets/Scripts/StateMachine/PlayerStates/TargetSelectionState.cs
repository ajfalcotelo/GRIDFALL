using System.Collections.Generic;
using UnityEngine;

public class TargetSelectionState : BaseState
{
    private readonly PlayerUnitController unitController;
    private readonly InputController inputController;
    private readonly TargetingSystem targetingSystem;
    private readonly MovementPreviewSystem movementPreviewSystem;
    private List<PathNode> plannedPath;
    private PathNode prevHoverNode;
    private bool hasHovered;

    public TargetSelectionState(
        StateMachine stateMachine,
        IUnitRoot unit,
        PlayerUnitController unitController,
        InputController inputController,
        TargetingSystem targetingSystem,
        MovementPreviewSystem movementPreviewSystem
    )
        : base(stateMachine, unit)
    {
        this.unitController = unitController;
        this.inputController = inputController;
        this.targetingSystem = targetingSystem;
        this.movementPreviewSystem = movementPreviewSystem;
    }

    public override void Enter()
    {
        inputController.EnableSelectionInputs();
        inputController.Click += OnClick;
        inputController.Hover += OnHover;
        inputController.Cancel += OnCancelSelection;
        TargetingData targetData = unitController.SelectedAction.GetTargetingData(unit);
        targetingSystem.HighlightSelectableNodes(unit, targetData);
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

        if (
            !targetingSystem.IsSelectedNodeValid(selectedNode)
            || !unitController.SelectedAction.CanRun(context)
        )
            return;

        targetingSystem.ClearSetTiles();
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
        if (targetingSystem.IsSelectedNodeValid(hoveredNode))
        {
            targetingSystem.HighlightHover(mouseWorldPosition);
        }
    }

    private void MoveHover(Vector3 mouseWorldPosition)
    {
        PathNode hoveredNode = GridManager.Instance.PathfindLayer.GetNode(mouseWorldPosition);
        if (
            hoveredNode == null
            || !hoveredNode.IsWalkable
            || !targetingSystem.IsSelectedNodeValid(hoveredNode)
        )
        {
            movementPreviewSystem.Clear();
            targetingSystem.ClearHover();
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

    private void PreviewMove(Vector3 mouseWorldPosition, PathNode hoveredNode)
    {
        List<PathNode> path = Pathfinding.FindPath(unit.CurrentNode.Position, hoveredNode.Position);
        movementPreviewSystem.RenderPreview(unit, path);
        targetingSystem.HighlightHover(mouseWorldPosition);
        plannedPath = path;
    }

    private void OnCancelSelection()
    {
        ChangeState(unitController.ActionSelectionState);
        targetingSystem.ClearSetTiles();
    }
}
