using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Tile clickTile;

    [SerializeField]
    private Tile hoverTile;

    [SerializeField]
    private Tilemap hoverTilemap;

    [SerializeField]
    private Tilemap clickTilemap;

    [SerializeField]
    private float moveSpeed = 5f;

    PlayerInputActions playerInputActions;
    Vector3Int prevHoverPosition;
    Vector3Int prevClickPosition;
    Vector3 targetPosition;
    bool isMoving = false;

    void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        playerInputActions.Player.Click.Enable();
        playerInputActions.Player.Hover.Enable();
        playerInputActions.Player.Click.performed += OnClick;
        playerInputActions.Player.Hover.performed += OnHover;
    }

    void OnDisable()
    {
        playerInputActions.Player.Click.performed -= OnClick;
        playerInputActions.Player.Hover.performed -= OnHover;
        playerInputActions.Player.Click.Disable();
        playerInputActions.Player.Hover.Disable();
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                Time.deltaTime * moveSpeed
            );

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                transform.position = targetPosition;
                clickTilemap.SetTile(clickTilemap.WorldToCell(targetPosition), null);
                isMoving = false;
            }
        }
    }

    void OnClick(InputAction.CallbackContext context)
    {
        var mousePosition = Mouse.current.position.ReadValue();
        var mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        var cellPosition = clickTilemap.WorldToCell(mouseWorldPosition);
        targetPosition = clickTilemap.GetCellCenterWorld(cellPosition);

        if (isMoving && prevClickPosition != cellPosition)
        {
            clickTilemap.SetTile(prevClickPosition, null);
        }

        isMoving = true;
        clickTilemap.SetTile(cellPosition, clickTile);
        prevClickPosition = cellPosition;
    }

    void OnHover(InputAction.CallbackContext context)
    {
        var mousePosition = context.ReadValue<Vector2>();
        var mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        var cellPosition = hoverTilemap.WorldToCell(mouseWorldPosition);

        if (cellPosition != prevHoverPosition)
        {
            hoverTilemap.SetTile(prevHoverPosition, null);
            hoverTilemap.SetTile(cellPosition, hoverTile);
            prevHoverPosition = cellPosition;
        }
    }
}
