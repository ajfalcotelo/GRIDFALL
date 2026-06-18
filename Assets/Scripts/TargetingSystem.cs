using System.Collections.Generic;
using System.Linq;
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
        PathNode sourceNode = GridManager.Instance.GetNode(unit.GridPosition);
        nodes = FindInRange(sourceNode, range).ToDictionary(e => e.Position, e => e);
        List<PathNode> nodeList = new();

        foreach (var node in nodes.Values)
        {
            Vector3 pos = GridManager.Instance.XYToWorldPos(node.Position.x, node.Position.y);
            highlightTilemap.SetTile(Vector3Int.RoundToInt(pos), highlightRuleTile);
        }
    }

    private List<PathNode> FindInRange(PathNode source, int range)
    {
        Queue<(PathNode node, int dist)> queue = new();
        HashSet<PathNode> visited = new();
        List<PathNode> inRangeNodes = new();

        queue.Enqueue((source, 0));
        visited.Add(source);

        while (queue.Any())
        {
            var (node, dist) = queue.Dequeue();

            inRangeNodes.Add(node);

            if (dist >= range)
                continue;

            foreach (
                PathNode neighbor in node.CardinalNeighbors.Where(node =>
                    node.IsWalkable && !visited.Contains(node)
                )
            )
            {
                visited.Add(neighbor);
                queue.Enqueue((neighbor, dist + 1));
            }
        }

        return inRangeNodes;
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
