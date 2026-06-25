using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [SerializeField]
    private Tilemap groundTilemap;

    [SerializeField]
    private Tilemap obstacleTilemap;

    public PathfindLayer PathfindLayer { get; private set; }
    public OccupancyLayer OccupancyLayer { get; private set; }

    public Tilemap GetGroundTilemap => groundTilemap;

    void Awake()
    {
        Instance = this;

        PathfindLayer = new(groundTilemap);
        OccupancyLayer = new(groundTilemap);

        PathfindLayer.BuildLayer(obstacleTilemap);
        OccupancyLayer.BuildLayer();
    }

    public Vector2 NodeToWorld(Vector2Int pos) =>
        pos + new Vector2(groundTilemap.cellBounds.min.x, groundTilemap.cellBounds.min.y);
}
