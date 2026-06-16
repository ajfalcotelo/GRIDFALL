using System;
using UnityEngine;
using UnityEngine.UI;

public class UIActionMenu : MonoBehaviour
{
    [SerializeField]
    private PlayerStateMachine stateMachine;

    [SerializeField]
    private Button moveButton;

    [SerializeField]
    private Button attackButton;

    [SerializeField]
    private Button endTurnButton;

    public event Action ButtonPressed = delegate { };

    public void SetActionMenuActive()
    {
        gameObject.SetActive(true);
    }

    public void DisableMoveButton() => moveButton.interactable = false;

    public void EnableMoveButton() => moveButton.interactable = true;

    public void DisableAttackButton() => attackButton.interactable = false;

    public void EnableAttackButton() => attackButton.interactable = true;

    public void OnMoveButtonPressed()
    {
        stateMachine.SelectedAction = stateMachine.MoveAction;
        ButtonPressed.Invoke();
        gameObject.SetActive(false);
    }

    public void OnAttackButtonPressed()
    {
        stateMachine.SelectedAction = stateMachine.AttackAction;
        ButtonPressed.Invoke();
        gameObject.SetActive(false);
    }

    public void OnEndTurnButtonPressed()
    {
        stateMachine.ChangeState(stateMachine.EndTurnState);
    }
}
