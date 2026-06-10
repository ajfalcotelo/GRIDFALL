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
    public event System.Action OnActionFinished = delegate { };

    [SerializeField]
    private Tile clickTile;

    [SerializeField]
    private Tile hoverTile;

    [SerializeField]
    private Tilemap hoverTilemap;

    [SerializeField]
    private Tilemap clickTilemap;

    [SerializeField]
    private float moveSpeed = 1f;

    [SerializeField]
    private InputController input;

    private Vector3Int prevHoverPosition;
    private Vector3Int prevClickPosition;
    private List<PathNode> paths;
    private bool isMoving = false;

    void Start()
    {
        transform.position = GridManager.Instance.GetGroundTilemap.GetCellCenterWorld(
            Vector3Int.RoundToInt(transform.position)
        );
    }

    void OnEnable()
    {
        input.Click += OnClick;
        input.Hover += OnHover;
    }

    void OnDisable()
    {
        input.Click -= OnClick;
        input.Hover -= OnHover;
    }

    public void SetActionMode(PlayerActionMode action)
    {
        switch (action)
        {
            case PlayerActionMode.Move:
                input.EnablePlayerInputActions();
                break;
            case PlayerActionMode.Attack:
            case PlayerActionMode.None:
                input.DisablePlayerInputActions();
                break;
        }
    }

    private void OnClick(Vector2 mousePosition)
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3Int cellPosition = clickTilemap.WorldToCell(mouseWorldPosition);

        if (isMoving && prevClickPosition != cellPosition)
        {
            clickTilemap.SetTile(prevClickPosition, null);
        }

        isMoving = true;
        clickTilemap.SetTile(cellPosition, clickTile);
        prevClickPosition = cellPosition;

        Vector2Int mouseXYPos = GridManager.Instance.WorldToXY(mouseWorldPosition);
        Vector2Int playerXYPos = GridManager.Instance.WorldToXY(transform.position);
        List<PathNode> path = Pathfinding.FindPath(playerXYPos, mouseXYPos);
        if (path == null)
            return;

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
        OnActionFinished.Invoke();
    }

    private void OnHover(Vector2 mousePosition)
    {
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
        isMoving = false;
        hoverTilemap.ClearAllTiles();
    }
}
