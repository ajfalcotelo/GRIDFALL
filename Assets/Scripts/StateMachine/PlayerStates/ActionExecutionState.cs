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
        stateMachine.ExecuteAction(new ActionContext(unit, stateMachine.SelectedTargetNode));
    }

    public override void Exit() { }
}
