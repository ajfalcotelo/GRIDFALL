using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public string GCostText { get; private set; }
    public string HCostText { get; private set; }
    public string FCostText { get; private set; }

    private readonly Vector2Int[] directions =
    {
        new(0, 1), // Up
        new(1, 1), // Up-Right
        new(1, 0), // Right
        new(1, -1), // Down-Right
        new(0, -1), // Down
        new(-1, -1), // Down-Left
        new(-1, 0), // Left
        new(-1, 1), // Up-Left
    };

    public Vector2Int Position { get; private set; }
    public List<PathNode> Neighbors { get; private set; }
    public PathNode Parent { get; set; }
    public int GCost { get; private set; }
    public int HCost { get; private set; }
    public int FCost => GCost + HCost;
    public bool IsWalkable { get; set; }

    public PathNode(Vector2Int position)
    {
        Position = position;
        IsWalkable = true;
    }

    public void SetGCost(int value)
    {
        GCost = value;
    }

    public void SetHCost(int value)
    {
        HCost = value;
    }

    public void ListNeighbors()
    {
        Neighbors = new();
        foreach (var dir in directions)
        {
            var node = GridManager.Instance.PathfindLayer.GetNode(Position + dir);
            if (node != null)
                Neighbors.Add(node);
        }
    }

    public int GetDistance(PathNode other)
    {
        var dist = new Vector2Int(
            Mathf.Abs(Position.x - other.Position.x),
            Mathf.Abs(Position.y - other.Position.y)
        );

        var lowest = Mathf.Min(dist.x, dist.y);
        var highest = Mathf.Max(dist.x, dist.y);

        var horizontalMovesRequired = highest - lowest;

        return lowest * MOVE_DIAGONAL_COST + horizontalMovesRequired * MOVE_STRAIGHT_COST;
    }
}
