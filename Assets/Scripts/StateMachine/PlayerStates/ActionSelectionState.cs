using UnityEngine;

public class ActionSelectionState : PlayerBaseState
{
    private readonly UIActionMenu actionMenu;

    public ActionSelectionState(PlayerStateMachine stateMachine, UIActionMenu actionMenu)
        : base(stateMachine)
    {
        this.actionMenu = actionMenu;
    }

    public override void Enter()
    {
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
