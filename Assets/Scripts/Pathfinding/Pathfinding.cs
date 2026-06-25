using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding
{
    // as of now, there are no exception handling for when clicking outside the tilemap bounds
    public static List<PathNode> FindPath(Vector2Int start, Vector2Int target)
    {
        PathNode startNode = GridManager.Instance.PathfindLayer.GetNode(start);
        PathNode targetNode = GridManager.Instance.PathfindLayer.GetNode(target);

        var openList = new List<PathNode> { startNode };
        var closedList = new List<PathNode>();

        while (openList.Any())
        {
            var current = openList[0];
            foreach (var node in openList)
            {
                if (
                    node.FCost < current.FCost
                    || node.FCost == current.FCost && node.HCost < current.HCost
                )
                    current = node;
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
            foreach (
                PathNode neighbor in current.AllNeighbors.Where(node =>
                    node.IsWalkable && !closedList.Contains(node)
                )
            )
            {
                var inSearch = openList.Contains(neighbor);

                int tentativeGCost = current.GetDistance(neighbor) + current.GCost;
                if (!inSearch || tentativeGCost < neighbor.GCost)
                {
                    neighbor.SetGCost(tentativeGCost);
                    neighbor.Parent = current;

                    if (!inSearch)
                    {
                        neighbor.SetHCost(neighbor.GetDistance(targetNode));
                        openList.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }
}
