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

    public void HighlightSelectableNodes(IUnitRoot unit, TargetingData data)
    {
        nodes = GetReachableNodes(unit, data).ToDictionary(e => e.Position, e => e);
        foreach (var node in nodes.Values)
        {
            Vector3 pos = GridManager.Instance.XYToWorldPos(node.Position.x, node.Position.y);
            highlightTilemap.SetTile(Vector3Int.RoundToInt(pos), highlightRuleTile);
        }
    }

    public List<PathNode> GetReachableNodes(IUnitRoot unit, TargetingData data)
    {
        return data.ActionType switch
        {
            ActionType.Move => BFSFind(unit.CurrentNode, data.Range),
            ActionType.Attack => ChebFind(unit.CurrentNode, data.Range),
            _ => null,
        };
    }

    public bool IsSelectedNodeValid(PathNode selectedNode)
    {
        PathNode node = nodes.TryGetValue(selectedNode.Position, out PathNode pathNode)
            ? pathNode
            : null;
        return node != null;
    }

    private List<PathNode> BFSFind(PathNode source, int range)
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
                    node.IsWalkable && !visited.Contains(node) && node.Occupant == null
                )
            )
            {
                visited.Add(neighbor);
                queue.Enqueue((neighbor, dist + 1));
            }
        }

        return inRangeNodes;
    }

    private List<PathNode> ChebFind(PathNode source, int range)
    {
        Vector2Int sourcePosition = source.Position;
        List<PathNode> inRangeNodes = new();

        for (int x = sourcePosition.x - range; x <= sourcePosition.x + range; x++)
        {
            for (int y = sourcePosition.y - range; y <= sourcePosition.y + range; y++)
            {
                var pos = new Vector2Int(x, y);
                var node = GridManager.Instance.GetNode(pos);
                if (node == null || sourcePosition == pos)
                    continue;
                inRangeNodes.Add(node);
            }
        }

        return inRangeNodes;
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
