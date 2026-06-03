using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public enum PlayerActionMode
{
    None,
    Move,
    Attack,
}

public class PlayerController : MonoBehaviour
{
    public event System.Action OnActionFinished;

    [SerializeField]
    private Tile clickTile;

    [SerializeField]
    private Tile hoverTile;

    [SerializeField]
    private float moveSpeed = 1f;

    private Tilemap hoverTilemap;
    private Tilemap clickTilemap;
    private PlayerInputActions playerInputActions;
    private Vector3Int prevHoverPosition;
    private List<PathNode> paths;

    void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        playerInputActions.Player.Click.performed += OnMove;
        playerInputActions.Player.Hover.performed += OnHover;
    }

    void OnDisable()
    {
        playerInputActions.Player.Click.performed -= OnMove;
        playerInputActions.Player.Hover.performed -= OnHover;
    }

    void Start()
    {
        hoverTilemap = GameObject.FindGameObjectWithTag("HoverTilemap").GetComponent<Tilemap>();
        clickTilemap = GameObject.FindGameObjectWithTag("ClickTilemap").GetComponent<Tilemap>();
        transform.position = GridManager.Instance.GetGroundTilemap.GetCellCenterWorld(
            Vector3Int.RoundToInt(transform.position)
        );
    }

    public void SetActionMode(PlayerActionMode action)
    {
        playerInputActions.Player.Click.Disable();
        playerInputActions.Player.Hover.Disable();

        switch (action)
        {
            case PlayerActionMode.Move:
                playerInputActions.Player.Hover.Enable();
                playerInputActions.Player.Click.Enable();
                break;
            case PlayerActionMode.Attack:
            case PlayerActionMode.None:
                playerInputActions.Player.Disable();
                break;
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3Int cellPosition = clickTilemap.WorldToCell(mouseWorldPosition);
        clickTilemap.SetTile(cellPosition, clickTile);

        Vector2Int mouseXYPos = GridManager.Instance.WorldToXY(mouseWorldPosition);
        Vector2Int playerXYPos = GridManager.Instance.WorldToXY(transform.position);
        List<PathNode> path = Pathfinding.FindPath(playerXYPos, mouseXYPos);

        if (path != null)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Vector3 start =
                    new Vector3(path[i].Position.x, path[i].Position.y, 0)
                    + GridManager.Instance.GetGroundTilemap.cellBounds.min
                    + Vector3.one * 0.5f;
                Vector3 end =
                    new Vector3(path[i + 1].Position.x, path[i + 1].Position.y, 0)
                    + GridManager.Instance.GetGroundTilemap.cellBounds.min
                    + Vector3.one * 0.5f;

                Debug.DrawLine(start, end, Color.green, 3f);
            }

            paths = path;
            StartCoroutine(FollowPath());
        }
    }

    private void OnHover(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = context.ReadValue<Vector2>();
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3Int cellPosition = hoverTilemap.WorldToCell(mouseWorldPosition);

        if (cellPosition != prevHoverPosition)
        {
            hoverTilemap.SetTile(prevHoverPosition, null);
            hoverTilemap.SetTile(cellPosition, hoverTile);
            prevHoverPosition = cellPosition;
        }
    }

    private IEnumerator FollowPath()
    {
        foreach (PathNode path in paths)
        {
            while (
                Vector2.Distance(
                    transform.position,
                    GridManager.Instance.XYToWorldPos(path.Position.x, path.Position.y)
                        + Vector2.one * 0.5f
                ) > 0.05f
            )
            {
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    GridManager.Instance.XYToWorldPos(path.Position.x, path.Position.y)
                        + Vector2.one * 0.5f,
                    moveSpeed * Time.deltaTime
                );

                yield return null;
            }
        }
        Vector2 targetTilePos = GridManager.Instance.XYToWorldPos(
            paths[^1].Position.x,
            paths[^1].Position.y
        );
        clickTilemap.SetTile(
            clickTilemap.WorldToCell(new Vector3(targetTilePos.x, targetTilePos.y)),
            null
        );
        clickTilemap.ClearAllTiles();
        hoverTilemap.ClearAllTiles();
        OnActionFinished?.Invoke();
    }
}
