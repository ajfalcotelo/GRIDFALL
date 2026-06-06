using UnityEngine;

public class UIActionMenu : MonoBehaviour
{
    public void SetActionMenuActive()
    {
        gameObject.SetActive(true);
    }

    public void OnMoveButtonPressed()
    {
        gameObject.SetActive(false);
        GameManager.Instance.SetCurrentAction(PlayerAction.Move);
        GameManager.Instance.SetPlayerState(PlayerState.SelectTarget);
    }

    public void OnAttackButtonPressed()
    {
        gameObject.SetActive(false);
    }
}
