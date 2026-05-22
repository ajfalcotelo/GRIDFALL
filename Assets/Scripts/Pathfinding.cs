using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding
{
    
    private static readonly Vector2Int[] directions =
    {
        // 4way directions
        // new(0, 1),
        // new(0, -1),
        // new(1, 0),
        // new(-1, 0)

        // 8way directions
        new (0, 1),
        new (-1, 0),
        new (0, -1),
        new (1, 0),
        new (1, 1),
        new (1, -1),
        new (-1, -1),
        new (-1, 1)
    };

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private readonly Grid grid;

    public Pathfinding(int width, int height, Vector3 originPosition)
    {
        grid = new(width, height, 1f, originPosition);
    }

    public Grid GetGrid()
    {
        return grid;
    }

    // as of now, there are no exception handling for when clicking outside the tilemap bounds
    public List<PathNode> FindPath(Vector2Int start, Vector2Int target)
    {
        PathNode startNode = grid.GetNode(start.x, start.y);
        PathNode targetNode = grid.GetNode(target.x, target.y);

        var openList = new List<PathNode> {startNode};
        var closedList = new List<PathNode>();

        var current = openList[0];
        current.SetGCost(0);
        current.SetHCost(GetDistance(startNode, targetNode));

        while (openList.Any())
        {
            foreach (var node in openList)
            {
                if (node.FCost < current.FCost || node.FCost == current.FCost && node.HCost < current.HCost) current = node;
            }

            openList.Remove(current);
            closedList.Add(current);

            // Get path from start node to target node
            if (current == targetNode)
            {
                var path = new List<PathNode>();
                PathNode currentPathNode = targetNode;
                while (currentPathNode != startNode)
                {
                    path.Add(currentPathNode);
                    currentPathNode = currentPathNode.Parent;
                }
                path.Add(startNode); // for debug, not needed for implementation
                path.Reverse();
                return path;
            }

            // Evaluate neighbor nodes
            foreach (var neighbor in GetNeighborList(current).Where(node => !closedList.Contains(node)))
            {
                if (closedList.Contains(neighbor)) continue;

                int tentativeGCost = GetDistance(current, neighbor) + current.GCost;
                if (tentativeGCost < current.GCost)
                {
                    neighbor.SetGCost(tentativeGCost);
                    neighbor.SetParent(current);

                    if (!openList.Contains(neighbor))
                    {
                        neighbor.SetHCost(GetDistance(neighbor, targetNode));
                        openList.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    private List<PathNode> GetNeighborList(PathNode current)
    {
        var neighborList = new List<PathNode>();

        foreach (var dir in directions.Select(dir => grid.GetNode(current.X + dir.x, current.Y + dir.y)).Where(node => node != null))
        {
            neighborList.Add(dir);
        }

        return neighborList;
    }

    private int GetDistance(PathNode a, PathNode b)
    {
        // 4d movement costs
        // return Mathf.Abs(a.X - b.X) + Mathf.Abs(a.Y - b.Y);

        // 8d movement costs
        var dist = new Vector2Int(Mathf.Abs(a.X - b.X), Mathf.Abs(a.Y - b.Y));

        var lowest = Mathf.Min(dist.x, dist.y);
        var highest = Mathf.Max(dist.x, dist.y);

        var horizontalMovesRequired = highest - lowest;

        return lowest * MOVE_DIAGONAL_COST + horizontalMovesRequired * MOVE_STRAIGHT_COST ;
    }

}
