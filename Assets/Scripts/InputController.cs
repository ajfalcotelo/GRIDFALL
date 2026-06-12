using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public event Action<Vector3> Click = delegate { };
    public event Action<Vector2> Hover = delegate { };
    public event Action Cancel = delegate { };

    private PlayerInputActions inputActions;

    void Awake()
    {
        inputActions = new();
    }

    public void EnablePlayerInputActions()
    {
        inputActions.Player.Enable();
        inputActions.Player.Cancel.performed += OnCancel;
    }

    public void DisablePlayerInputActions()
    {
        inputActions.Player.Cancel.performed -= OnCancel;
        inputActions.Player.Disable();
    }

    public void EnableSelectionInputs()
    {
        inputActions.Player.Click.performed += OnClick;
        inputActions.Player.Hover.performed += OnHover;
    }

    public void DisableSelectionInputs()
    {
        inputActions.Player.Click.performed -= OnClick;
        inputActions.Player.Hover.performed -= OnHover;
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Click.Invoke(pos);
    }

    private void OnHover(InputAction.CallbackContext context) =>
        Hover.Invoke(context.ReadValue<Vector2>());

    private void OnCancel(InputAction.CallbackContext context) => Cancel.Invoke();
}
