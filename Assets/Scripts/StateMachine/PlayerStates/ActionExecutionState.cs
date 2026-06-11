public class ActionExecutionState : PlayerBaseState
{
    private IUnit unit;

    public ActionExecutionState(PlayerStateMachine stateMachine, IUnit unit)
        : base(stateMachine)
    {
        this.unit = unit;
    }

    public override void Enter()
    {
        var context = new ActionContext(unit, stateMachine.SelectedTargetNode);
        stateMachine.SelectedAction.Run(stateMachine, context, stateMachine.OnActionExecuted);
    }

    public override void Exit() { }
}
