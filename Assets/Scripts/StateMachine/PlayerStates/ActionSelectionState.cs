public class ActionSelectionState : BaseState
{
    private readonly PlayerUnitController unitController;
    private readonly InputController inputController;
    private readonly UIActionMenu actionMenu;

    public ActionSelectionState(
        StateMachine stateMachine,
        PlayerUnitRoot unit,
        PlayerUnitController unitController,
        InputController inputController,
        UIActionMenu actionMenu
    )
        : base(stateMachine, unit)
    {
        this.unitController = unitController;
        this.inputController = inputController;
        this.actionMenu = actionMenu;
    }

    public override void Enter()
    {
        if (unit.MoveRange.CurrentMoveRange <= 0)
            actionMenu.DisableMoveButton();
        else
            actionMenu.EnableMoveButton();

        if (unit.Stats.ActionCount <= 0)
            actionMenu.DisableAttackButton();
        else
            actionMenu.EnableAttackButton();

        inputController.EnablePlayerInputActions();
        actionMenu.SetActionMenuActive();
        actionMenu.ButtonPressed += OnButtonPressed;
    }

    public override void Exit()
    {
        actionMenu.ButtonPressed -= OnButtonPressed;
    }

    private void OnButtonPressed()
    {
        ChangeState(unitController.TargetSelecionState);
    }
}
