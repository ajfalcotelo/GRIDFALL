using UnityEngine;

public class UIActionMenu : MonoBehaviour
{
    public void SetActionMenuActive()
    {
        GameManager.Instance.PlayerController.SetActionMode(PlayerActionMode.None);
        gameObject.SetActive(true);
    }

    public void OnMoveButtonPressed()
    {
        GameManager.Instance.PlayerController.SetActionMode(PlayerActionMode.Move);
        gameObject.SetActive(false);
    }

    public void OnAttackButtonPressed()
    {
        GameManager.Instance.PlayerController.SetActionMode(PlayerActionMode.Attack);
        gameObject.SetActive(false);
    }
}
