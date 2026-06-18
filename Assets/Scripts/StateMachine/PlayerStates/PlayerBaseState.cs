public abstract class PlayerBaseState : IState
{
    protected StateMachine stateMachine;
    protected PlayerUnitRoot unit;
    protected PlayerUnitController unitController;

    public PlayerBaseState(
        StateMachine stateMachine,
        PlayerUnitRoot unit,
        PlayerUnitController unitController
    )
    {
        this.stateMachine = stateMachine;
        this.unit = unit;
        this.unitController = unitController;
    }

    protected PlayerBaseState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public abstract void Enter();
    public abstract void Exit();
}
