using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public event Action<Vector3> Click = delegate { };
    public event Action<Vector2> Hover = delegate { };

    private PlayerInputActions inputActions;

    void Awake()
    {
        inputActions = new();
    }

    public void EnablePlayerInputActions()
    {
        inputActions.Player.Enable();
        inputActions.Player.Click.performed += OnClick;
        inputActions.Player.Hover.performed += OnHover;
    }

    public void DisablePlayerInputActions()
    {
        inputActions.Player.Click.performed -= OnClick;
        inputActions.Player.Hover.performed -= OnHover;
        inputActions.Player.Disable();
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Click.Invoke(pos);
    }

    private void OnHover(InputAction.CallbackContext context) =>
        Hover.Invoke(context.ReadValue<Vector2>());
}
