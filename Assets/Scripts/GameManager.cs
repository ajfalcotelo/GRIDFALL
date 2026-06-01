using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private UIActionMenu actionMenu;

    [SerializeField]
    private PlayerController playerController;

    void Start()
    {
        playerController.OnActionFinished += EndTurn;
    }

    private void EndTurn()
    {
        actionMenu.SetActionMenuActive();
    }
}
