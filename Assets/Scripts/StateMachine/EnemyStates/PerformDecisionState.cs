public class PerformDecisionState : BaseState
{
    private readonly EnemyUnitController unitController;

    public PerformDecisionState(
        StateMachine stateMachine,
        IUnitRoot unit,
        EnemyUnitController unitController
    )
        : base(stateMachine, unit)
    {
        this.unitController = unitController;
    }

    public override void Enter()
    {
        unitController.SelectedAction.Run(
            unitController,
            new ActionContext(unit, unitController.SelectedTargetNode),
            new System.Action(() => ChangeState(unitController.DecisionState))
        );
    }

    public override void Exit() { }
}
