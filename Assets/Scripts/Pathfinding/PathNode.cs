using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PathNode
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public string GCostText { get; private set; }
    public string HCostText { get; private set; }
    public string FCostText { get; private set; }

    private readonly Vector2Int[] cardinalDirections =
    {
        new(0, 1), // Up
        new(1, 0), // Right
        new(0, -1), // Down
        new(-1, 0), // Left
    };

    private readonly Vector2Int[] diagonalDirections =
    {
        new(1, 1), // Up-Right
        new(1, -1), // Down-Right
        new(-1, -1), // Down-Left
        new(-1, 1), // Up-Left
    };

    public Vector2Int Position { get; private set; }
    public List<PathNode> AllNeighbors { get; private set; }
    public List<PathNode> CardinalNeighbors { get; private set; }
    public PathNode Parent { get; set; }
    public int GCost { get; private set; }
    public int HCost { get; private set; }
    public int FCost => GCost + HCost;
    public bool IsWalkable { get; set; }
    public IUnitRoot Occupant { get; set; }

    public PathNode(Vector2Int position)
    {
        Position = position;
        IsWalkable = true;
        Occupant = null;
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
        List<PathNode> diagonalNeighbors = new();
        CardinalNeighbors = new();

        foreach (
            var dir in cardinalDirections
                .Select(dir => GridManager.Instance.GetNode(Position + dir))
                .Where(node => node != null)
        )
        {
            CardinalNeighbors.Add(dir);
        }

        foreach (
            var dir in diagonalDirections
                .Select(dir => GridManager.Instance.GetNode(Position + dir))
                .Where(node => node != null)
        )
        {
            diagonalNeighbors.Add(dir);
        }

        AllNeighbors = new(CardinalNeighbors.Count + diagonalNeighbors.Count);
        AllNeighbors.AddRange(CardinalNeighbors);
        AllNeighbors.AddRange(diagonalNeighbors);
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
