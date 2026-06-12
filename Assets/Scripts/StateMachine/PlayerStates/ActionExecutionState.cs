public class ActionExecutionState : PlayerBaseState
{
    private readonly IUnit unit;
    private readonly InputController inputController;

    public ActionExecutionState(
        PlayerStateMachine stateMachine,
        IUnit unit,
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
        stateMachine.SelectedAction.Run(stateMachine, context, stateMachine.OnActionExecuted);
    }

    public override void Exit() { }
}
