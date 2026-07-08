public class ActionExecutionState : BaseState
{
    private readonly PlayerUnitController unitController;
    private readonly InputController inputController;

    public ActionExecutionState(
        StateMachine stateMachine,
        IUnitRoot unit,
        PlayerUnitController unitController,
        InputController inputController
    )
        : base(stateMachine, unit)
    {
        this.inputController = inputController;
        this.unitController = unitController;
    }

    public override void Enter()
    {
        inputController.DisablePlayerInputActions();

        if (
            (unit.Stats.ActionCount <= 0 && unit.MoveRange.CurrentMoveRange > 0)
            || unit.Stats.ActionCount > 0
        )
            unitController.SelectedAction.Run(
                unitController,
                new ActionContext()
                {
                    TargetUnit = GridManager.Instance.OccupancyLayer.GetNode(
                        unitController.SelectedTargetNode.Position
                    ),
                    TargetNode = unitController.SelectedTargetNode,
                    Path = unitController.SelectedPath,
                },
                new System.Action(() => ChangeState(unitController.ActionSelectionState))
            );
    }

    public override void Exit() { }
}
