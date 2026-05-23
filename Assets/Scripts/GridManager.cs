using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    public Dictionary<Vector2Int, PathNode> Nodes { get; private set; }

    private PlayerInputActions inputActions;
    private readonly NodeGrid nodeGrid = new();

    [SerializeField]
    private Tilemap groundTilemap;

    [SerializeField]
    private Tilemap obstacleTilemap;

    void Awake()
    {
        Instance = this;
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Player.Click.Enable();
        inputActions.Player.Click.performed += OnClick;
    }

    void OnDisable()
    {
        inputActions.Player.Click.performed -= OnClick;
        inputActions.Player.Click.Disable();
    }

    void Start()
    {
        BoundsInt cellBounds = groundTilemap.cellBounds;
        Nodes = nodeGrid.BuildGrid(cellBounds.size.x, cellBounds.size.y);

        // loads all neighbor of every node
        foreach (var node in Nodes.Values)
            node.ListNeighbors();

        foreach (Vector3Int obstaclePos in obstacleTilemap.cellBounds.allPositionsWithin)
        {
            if (obstacleTilemap.HasTile(obstaclePos))
            {
                WorldToXY(obstaclePos, out int x, out int y);
                GetNode(new Vector2Int(x, y)).IsWalkable = false;
            }
        }

        ShowDebug(true);
    }

    public PathNode GetNode(Vector2Int position) =>
        Nodes.TryGetValue(position, out PathNode pathNode) ? pathNode : null;

    public void WorldToXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - groundTilemap.cellBounds.min).x);
        y = Mathf.FloorToInt((worldPosition - groundTilemap.cellBounds.min).y);
    }

    private void ShowDebug(bool show)
    {
        int width = groundTilemap.cellBounds.size.x;
        int height = groundTilemap.cellBounds.size.y;

        if (show)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GameObject gameObject = new("World_Text", typeof(TextMeshPro));
                    Transform transform = gameObject.transform;
                    transform.SetParent(null, false);
                    transform.localPosition = XYToWorldPos(x, y) + Vector2.one * 0.5f;
                    TextMeshPro textMesh = gameObject.GetComponent<TextMeshPro>();
                    textMesh.alignment = TextAlignmentOptions.Center;
                    textMesh.text = GetNode(new Vector2Int(x, y)).IsWalkable ? null : "X";
                    textMesh.fontSize = 4;
                    textMesh.color = Color.white;

                    Debug.DrawLine(XYToWorldPos(x, y), XYToWorldPos(x + 1, y), Color.white, 100f);
                    Debug.DrawLine(XYToWorldPos(x, y), XYToWorldPos(x, y + 1), Color.white, 100f);
                }
            }
            Debug.DrawLine(XYToWorldPos(0, height), XYToWorldPos(width, height), Color.white, 100f);
            Debug.DrawLine(XYToWorldPos(width, 0), XYToWorldPos(width, height), Color.white, 100f);
        }
    }

    void OnClick(InputAction.CallbackContext callbackContext)
    {
        var mousePosition = Mouse.current.position.ReadValue();
        var mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        WorldToXY(mouseWorldPosition, out int x, out int y);
        List<PathNode> path = Pathfinding.FindPath(Vector2Int.zero, new Vector2Int(x, y));
        if (path != null)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Vector3 start =
                    new Vector3(path[i].Position.x, path[i].Position.y, 0)
                    + groundTilemap.cellBounds.min
                    + Vector3.one * 0.5f;
                Vector3 end =
                    new Vector3(path[i + 1].Position.x, path[i + 1].Position.y, 0)
                    + groundTilemap.cellBounds.min
                    + Vector3.one * 0.5f;

                Debug.DrawLine(start, end, Color.green, 3f);
            }
        }
    }

    private Vector2 XYToWorldPos(int x, int y)
    {
        return new Vector2(x, y)
            + new Vector2(groundTilemap.cellBounds.min.x, groundTilemap.cellBounds.min.y);
    }
}
