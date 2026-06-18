public class ActionExecutionState : PlayerBaseState
{
    private readonly InputController inputController;

    public ActionExecutionState(
        StateMachine stateMachine,
        PlayerUnitRoot unit,
        PlayerUnitController unitController,
        InputController inputController
    )
        : base(stateMachine, unit, unitController)
    {
        this.inputController = inputController;
    }

    public override void Enter()
    {
        var context = new ActionContext(unit, unitController.SelectedTargetNode);
        inputController.DisablePlayerInputActions();

        if (
            (unit.Stats.ActionCount <= 0 && unit.MoveRange.CurrentMoveRange > 0)
            || unit.Stats.ActionCount > 0
        )
            unitController.SelectedAction.Run(
                unitController,
                context,
                new System.Action(() =>
                    stateMachine.ChangeState(unitController.ActionSelectionState)
                )
            );

        if (unitController.SelectedAction != unitController.MoveAction)
            unit.Stats.DecrementActionCount();
    }

    public override void Exit() { }
}
