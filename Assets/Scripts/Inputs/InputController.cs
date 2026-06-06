using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public event Action<PathNode> SelectedNode;
    private PlayerInputActions inputActions;

    void Awake()
    {
        inputActions = new();
    }

    void OnEnable()
    {
        inputActions.Player.Click.performed += HandleTargetting;
    }

    void OnDisable()
    {
        inputActions.Player.Click.performed -= HandleTargetting;
    }

    public void Activate()
    {
        inputActions.Player.Enable();
    }

    public void Deactivate()
    {
        inputActions.Player.Disable();
    }

    private void HandleTargetting(InputAction.CallbackContext ctx)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        SelectedNode?.Invoke(GridManager.Instance.GetNode(mousePos));
        GameManager.Instance.SetPlayerState(PlayerState.PerformAction);
    }
}
