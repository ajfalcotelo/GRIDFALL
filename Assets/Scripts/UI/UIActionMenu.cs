using UnityEngine;

public class UIActionMenu : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;

    public void SetActionMenuActive()
    {
        playerController.SetActionMode(PlayerActionMode.None);
        gameObject.SetActive(true);
    }

    public void OnMoveButtonPressed()
    {
        playerController.SetActionMode(PlayerActionMode.Move);
        gameObject.SetActive(false);
    }

    public void OnAttackButtonPressed()
    {
        playerController.SetActionMode(PlayerActionMode.Attack);
        gameObject.SetActive(false);
    }
}
