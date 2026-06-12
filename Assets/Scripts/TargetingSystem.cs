using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TargetingSystem : MonoBehaviour
{
    [SerializeField]
    private Tilemap hoverTilemap;

    [SerializeField]
    private Tile hoverTile;

    [SerializeField]
    private Tilemap highlightTilemap;

    [SerializeField]
    private RuleTile highlightRuleTile;

    [SerializeField]
    private Tilemap obstacleTilemap;

    private Vector3Int prevHoverPosition;

    private Dictionary<Vector2Int, PathNode> nodes;

    public void HighlightSelectableNodes(IUnit unit, int range)
    {
        nodes = new();
        List<PathNode> nodeList = new();

        PathNode sourceNode = GridManager.Instance.GetNode(unit.GridPosition);
        Vector2Int sourcePosition = sourceNode.Position;

        for (int x = sourcePosition.x - range; x <= sourcePosition.x + range; x++)
        {
            for (int y = sourcePosition.y - range; y <= sourcePosition.y + range; y++)
            {
                var node = GridManager.Instance.GetNode(new Vector2Int(x, y));
                var tileWorldPos = GridManager.Instance.XYToWorldPos(x, y);
                if (node == null || obstacleTilemap.HasTile(Vector3Int.RoundToInt(tileWorldPos)))
                    continue;
                nodes.Add(new Vector2Int(x, y), node);
                nodeList.Add(node);
            }
        }

        foreach (var node in nodeList)
        {
            Vector3 pos = GridManager.Instance.XYToWorldPos(node.Position.x, node.Position.y);
            highlightTilemap.SetTile(Vector3Int.RoundToInt(pos), highlightRuleTile);
        }
    }

    public bool IsSelectedNodeValid(PathNode selectedNode)
    {
        PathNode node = nodes.TryGetValue(selectedNode.Position, out PathNode pathNode)
            ? pathNode
            : null;
        return node != null;
    }

    public void ClearSetTiles()
    {
        highlightTilemap.ClearAllTiles();
        hoverTilemap.ClearAllTiles();
    }

    public void HighlightMouseHover(Vector2 mousePosition)
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
}
