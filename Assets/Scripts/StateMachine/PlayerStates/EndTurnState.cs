using UnityEngine;

public class EndTurnState : PlayerBaseState
{
    public EndTurnState(
        StateMachine stateMachine,
        PlayerUnitRoot unit,
        PlayerUnitController unitController
    )
        : base(stateMachine, unit, unitController) { }

    public override void Enter()
    {
        unit.MoveRange.ResetMoveRange();
        unit.Stats.ResetActionCount();
        stateMachine.ChangeState(unitController.ActionSelectionState); // temp
    }

    public override void Exit() { }
}
