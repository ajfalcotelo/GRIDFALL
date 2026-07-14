public class EndTurnState : BaseState
{
    private readonly BattleManager battleManager;

    public EndTurnState(StateMachine stateMachine, IUnitRoot unit, BattleManager battleManager)
        : base(stateMachine, unit)
    {
        this.battleManager = battleManager;
    }

    public override void Enter()
    {
        unit.MoveRange.ResetMoveRange();
        unit.Stats.ResetActionCount();
        unit.StatusEffects.TickTurnEnd();
        battleManager.NextUnitTurn();
    }

    public override void Exit() { }
}
