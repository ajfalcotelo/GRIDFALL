using UnityEngine;

public class ActionSelectionState : PlayerBaseState
{
    private readonly UIActionMenu actionMenu;
    private readonly InputController inputController;

    public ActionSelectionState(
        PlayerStateMachine stateMachine,
        UIActionMenu actionMenu,
        InputController inputController
    )
        : base(stateMachine)
    {
        this.actionMenu = actionMenu;
        this.inputController = inputController;
    }

    public override void Enter()
    {
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
