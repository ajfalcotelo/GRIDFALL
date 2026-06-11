using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private UIActionMenu actionMenu;

    [SerializeField]
    private PlayerStateMachine playerStateMachine;

    void Start()
    {
        playerStateMachine.OnActionExecuted += EndTurn;
    }

    private void EndTurn()
    {
        playerStateMachine.SetState(playerStateMachine.ActionSelectionState);
    }
}
