public class EndTurnState : PlayerBaseState
{
    private readonly PlayerUnit unit;

    public EndTurnState(PlayerStateMachine stateMachine, PlayerUnit unit)
        : base(stateMachine)
    {
        this.unit = unit;
    }

    public override void Enter()
    {
        unit.MoveRange.ResetMoveRange();
        unit.ResetActionCount();
        stateMachine.ChangeState(stateMachine.ActionSelectionState); // temp
    }

    public override void Exit() { }
}
