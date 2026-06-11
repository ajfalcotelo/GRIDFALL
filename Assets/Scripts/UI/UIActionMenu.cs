using System;
using UnityEngine;

public class UIActionMenu : MonoBehaviour
{
    [SerializeField]
    private PlayerStateMachine stateMachine;

    public event Action ButtonPressed = delegate { };

    public void SetActionMenuActive()
    {
        gameObject.SetActive(true);
    }

    public void OnMoveButtonPressed()
    {
        stateMachine.SelectedAction = stateMachine.MoveAction;
        ButtonPressed.Invoke();
        gameObject.SetActive(false);
    }

    public void OnAttackButtonPressed()
    {
        gameObject.SetActive(false);
        ButtonPressed.Invoke();
    }
}
