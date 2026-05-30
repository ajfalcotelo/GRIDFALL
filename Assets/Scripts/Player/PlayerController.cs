using System.Collections;
using System.Collections.Generic;
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
    private GridManager gridManager;

    [SerializeField]
    private float moveSpeed = 5f;

    private PlayerInputActions playerInputActions;
    private Vector3Int prevHoverPosition;
    private Vector3Int prevClickPosition;
    private List<PathNode> paths;
    private bool isMoving = false;

    void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        playerInputActions.Player.Click.Enable();
        playerInputActions.Player.Hover.Enable();
        playerInputActions.Player.Click.performed += OnClickMove;
        playerInputActions.Player.Hover.performed += OnHover;
    }

    void OnDisable()
    {
        playerInputActions.Player.Click.performed -= OnClickMove;
        playerInputActions.Player.Hover.performed -= OnHover;
        playerInputActions.Player.Click.Disable();
        playerInputActions.Player.Hover.Disable();
    }

    void Start()
    {
        transform.position = gridManager.GetGroundTilemap.GetCellCenterWorld(
            Vector3Int.RoundToInt(transform.position)
        );
    }

    private IEnumerator FollowPath()
    {
        foreach (PathNode path in paths)
        {
            while (
                Vector2.Distance(
                    transform.position,
                    gridManager.XYToWorldPos(path.Position.x, path.Position.y) + Vector2.one * 0.5f
                ) > 0.05f
            )
            {
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    gridManager.XYToWorldPos(path.Position.x, path.Position.y) + Vector2.one * 0.5f,
                    moveSpeed * Time.deltaTime
                );

                yield return null;
            }
        }
        Vector2 targetTilePos = gridManager.XYToWorldPos(
            paths[^1].Position.x,
            paths[^1].Position.y
        );
        clickTilemap.SetTile(
            clickTilemap.WorldToCell(new Vector3(targetTilePos.x, targetTilePos.y)),
            null
        );
        isMoving = false;
    }

    private void OnClickMove(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3Int cellPosition = clickTilemap.WorldToCell(mouseWorldPosition);

        if (isMoving && prevClickPosition != cellPosition)
        {
            clickTilemap.SetTile(prevClickPosition, null);
        }

        isMoving = true;
        clickTilemap.SetTile(cellPosition, clickTile);
        prevClickPosition = cellPosition;

        Vector2Int mouseXYPos = gridManager.WorldToXY(mouseWorldPosition);
        Vector2Int playerXYPos = gridManager.WorldToXY(transform.position);
        List<PathNode> path = Pathfinding.FindPath(playerXYPos, mouseXYPos);
        if (path != null)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Vector3 start =
                    new Vector3(path[i].Position.x, path[i].Position.y, 0)
                    + gridManager.GetGroundTilemap.cellBounds.min
                    + Vector3.one * 0.5f;
                Vector3 end =
                    new Vector3(path[i + 1].Position.x, path[i + 1].Position.y, 0)
                    + gridManager.GetGroundTilemap.cellBounds.min
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
}
