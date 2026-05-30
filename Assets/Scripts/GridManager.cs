using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    public Dictionary<Vector2Int, PathNode> Nodes { get; private set; }

    private readonly NodeGrid nodeGrid = new();

    [SerializeField]
    private Tilemap groundTilemap;

    [SerializeField]
    private Tilemap obstacleTilemap;

    public Tilemap GetGroundTilemap => groundTilemap;

    void Awake()
    {
        Instance = this;
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
                Vector2Int pos = WorldToXY(obstaclePos);
                GetNode(pos).IsWalkable = false;
            }
        }

        ShowGridDebug(true);
    }

    public PathNode GetNode(Vector2Int position) =>
        Nodes.TryGetValue(position, out PathNode pathNode) ? pathNode : null;

    public Vector2Int WorldToXY(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition - groundTilemap.cellBounds.min).x);
        int y = Mathf.FloorToInt((worldPosition - groundTilemap.cellBounds.min).y);

        return new Vector2Int(x, y);
    }

    private void ShowGridDebug(bool show)
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

    public Vector2 XYToWorldPos(int x, int y)
    {
        return new Vector2(x, y)
            + new Vector2(groundTilemap.cellBounds.min.x, groundTilemap.cellBounds.min.y);
    }
}
