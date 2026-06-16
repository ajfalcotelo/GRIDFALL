public class ActionSelectionState : PlayerBaseState
{
    private readonly PlayerUnit unit;
    private readonly InputController inputController;
    private readonly UIActionMenu actionMenu;

    public ActionSelectionState(
        PlayerStateMachine stateMachine,
        PlayerUnit unit,
        InputController inputController,
        UIActionMenu actionMenu
    )
        : base(stateMachine)
    {
        this.unit = unit;
        this.inputController = inputController;
        this.actionMenu = actionMenu;
    }

    public override void Enter()
    {
        if (unit.MoveRange.CurrentMoveRange <= 0)
            actionMenu.DisableMoveButton();
        else
            actionMenu.EnableMoveButton();

        if (unit.ActionCount <= 0)
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
        stateMachine.ChangeState(stateMachine.TargetSelecionState);
    }
}
