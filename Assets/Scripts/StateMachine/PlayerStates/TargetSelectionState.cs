using UnityEngine;

public class TargetSelectionState : PlayerBaseState
{
    private readonly IUnit unit;
    private readonly TargetingSystem targetingSystem;
    private readonly InputController inputController;

    public TargetSelectionState(
        PlayerStateMachine stateMachine,
        IUnit unit,
        InputController inputController,
        TargetingSystem targetingSystem
    )
        : base(stateMachine)
    {
        this.unit = unit;
        this.inputController = inputController;
        this.targetingSystem = targetingSystem;
    }

    public override void Enter()
    {
        inputController.EnableSelectionInputs();
        inputController.Click += OnClick;
        inputController.Hover += OnHover;
        var range =
            stateMachine.SelectedAction == stateMachine.MoveAction ? unit.Speed : unit.Range; // temp solution
        targetingSystem.HighlightSelectableNodes(unit, range);
    }

    public override void Exit()
    {
        inputController.Click -= OnClick;
        inputController.Hover -= OnHover;
        inputController.DisableSelectionInputs();
    }

    private void OnClick(Vector3 mousePos)
    {
        PathNode selectedNode = GridManager.Instance.GetNode(mousePos);
        if (!targetingSystem.IsSelectedNodeValid(selectedNode))
            return;
        targetingSystem.ClearSetTiles();
        stateMachine.SelectedTargetNode = selectedNode;
        stateMachine.ChangeState(stateMachine.ActionExecutionState);
    }

    private void OnHover(Vector2 mousePosition)
    {
        targetingSystem.HighlightMouseHover(mousePosition);
    }
}
