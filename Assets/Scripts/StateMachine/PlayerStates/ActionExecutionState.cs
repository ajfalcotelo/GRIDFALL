public class ActionExecutionState : PlayerBaseState
{
    private readonly PlayerUnit unit;
    private readonly InputController inputController;

    public ActionExecutionState(
        PlayerStateMachine stateMachine,
        PlayerUnit unit,
        InputController inputController
    )
        : base(stateMachine)
    {
        this.unit = unit;
        this.inputController = inputController;
    }

    public override void Enter()
    {
        var context = new ActionContext(unit, stateMachine.SelectedTargetNode);
        inputController.DisablePlayerInputActions();

        if ((unit.ActionCount <= 0 && unit.MoveRange.CurrentMoveRange > 0) || unit.ActionCount > 0)
            stateMachine.SelectedAction.Run(
                stateMachine,
                context,
                new System.Action(() => stateMachine.ChangeState(stateMachine.ActionSelectionState))
            );

        if (stateMachine.SelectedAction != stateMachine.MoveAction)
            unit.DecrementActionCount();
    }

    public override void Exit() { }
}
