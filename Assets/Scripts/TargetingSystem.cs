using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem
{
    private readonly Dictionary<Vector2Int, PathNode> nodes;

    public TargetingSystem()
    {
        nodes = new();
    }

    public PathNode ValidateSelectedNode(PathNode selectedNode)
    {
        PathNode node = nodes.TryGetValue(selectedNode.Position, out PathNode pathNode)
            ? pathNode
            : null;
        return node;
    }

    public void GetPossibleTargetNodes()
    {
        var nodeList = new List<PathNode>();

        // Get PathNode and Range from GameManager current unit
        PathNode sourceNode = GridManager.Instance.GetNode(
            GameManager.Instance.CurrentUnit.transform.position
        );
        Vector2Int sourcePosition = sourceNode.Position;
        int range = sourceNode.Occupant.GetComponent<Stats>().Range;

        for (int x = sourcePosition.x - range; x <= sourcePosition.x + range; x++)
        {
            for (int y = sourcePosition.y - range; y <= sourcePosition.y + range; y++)
            {
                var node = GridManager.Instance.GetNode(new Vector2Int(x, y));
                if (node == null)
                    continue;
                nodeList.Add(node);
            }
        }

        foreach (var node in nodeList)
        {
            nodes.Add(node.Position, node);
        }
    }

    public void DisplayPossibleTargetNodes()
    {
        // Display valid target nodes
        foreach (var nodePos in nodes.Keys)
        {
            Vector2 pos = GridManager.Instance.XYToWorldPos(nodePos.x, nodePos.y);
            GridManager.Instance.GetHighlightTilemap.SetTile(
                Vector3Int.RoundToInt(pos),
                GridManager.Instance.GetHighlightRuleTile
            );
        }
    }
}
