using UnityEngine;

public class TargetSelectionState : BaseState
{
    private readonly PlayerUnitController unitController;
    private readonly TargetingSystem targetingSystem;
    private readonly InputController inputController;

    public TargetSelectionState(
        StateMachine stateMachine,
        IUnitRoot unit,
        PlayerUnitController unitController,
        InputController inputController,
        TargetingSystem targetingSystem
    )
        : base(stateMachine, unit)
    {
        this.unitController = unitController;
        this.inputController = inputController;
        this.targetingSystem = targetingSystem;
    }

    public override void Enter()
    {
        inputController.EnableSelectionInputs();
        inputController.Click += OnClick;
        inputController.Hover += OnHover;
        TargetingData targetData = unitController.SelectedAction.GetTargetingData(unit);
        targetingSystem.HighlightSelectableNodes(unit, targetData);
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
        ActionContext context = new(unit, selectedNode);

        if (
            !targetingSystem.IsSelectedNodeValid(selectedNode)
            || !unitController.SelectedAction.CanRun(context)
        )
            return;

        targetingSystem.ClearSetTiles();
        unitController.SelectedTargetNode = selectedNode;
        ChangeState(unitController.ActionExecutionState);
    }

    private void OnHover(Vector2 mousePosition)
    {
        targetingSystem.HighlightMouseHover(mousePosition);
    }
}
