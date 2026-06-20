public abstract class BaseState : IState
{
    protected StateMachine stateMachine;
    protected IUnitRoot unit;

    public BaseState(StateMachine stateMachine, IUnitRoot unit)
    {
        this.stateMachine = stateMachine;
        this.unit = unit;
    }

    protected void ChangeState(IState state)
    {
        stateMachine.ChangeState(state);
    }

    public abstract void Enter();
    public abstract void Exit();
}
