public class StateMachine
{
    private IState currentState;

    public void SetState(IState state)
    {
        currentState = state;
        currentState.Enter();
    }

    public void ChangeState(IState state)
    {
        currentState.Exit();
        currentState = state;
        currentState.Enter();
    }
}
