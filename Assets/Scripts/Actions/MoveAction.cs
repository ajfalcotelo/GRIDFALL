using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    private readonly float moveSpeed = 5f;

    public MoveAction(IUnitRoot unit)
        : base(unit) { }

    public override bool CanRun(ActionContext context)
    {
        return context.TargetNode != null;
    }

    public override List<PathNode> GetReachableNodes()
    {
        Queue<(PathNode node, int dist)> queue = new();
        HashSet<PathNode> visited = new();
        List<PathNode> inRangeNodes = new();

        queue.Enqueue((actor.CurrentNode, 0));
        visited.Add(actor.CurrentNode);

        while (queue.Count > 0)
        {
            var (node, dist) = queue.Dequeue();

            inRangeNodes.Add(node);

            if (dist >= actor.MoveRange.CurrentMoveRange)
                continue;

            foreach (PathNode neighbor in node.Neighbors)
            {
                if (
                    neighbor.IsWalkable
                    && !visited.Contains(neighbor)
                    && GridManager.Instance.OccupancyLayer.GetNode(neighbor.Position) == null
                )
                {
                    visited.Add(neighbor);
                    queue.Enqueue((neighbor, dist + 1));
                }
            }
        }

        return inRangeNodes;
    }

    protected override IEnumerator Execute(ActionContext context)
    {
        var sourcePos = actor.CurrentNode.Position;
        var targetPos = context.TargetNode.Position;
        List<PathNode> paths = context.Path;
        if (paths == null)
            yield break;

        GridManager.Instance.OccupancyLayer.SetNode(sourcePos, null);
        foreach (PathNode path in paths)
        {
            while (
                Vector2.Distance(
                    actor.GameObject.transform.position,
                    GridManager.Instance.NodeToWorld(path.Position) + Vector2.one * 0.5f
                ) > 0.05f
            )
            {
                actor.GameObject.transform.position = Vector2.MoveTowards(
                    actor.GameObject.transform.position,
                    GridManager.Instance.NodeToWorld(path.Position) + Vector2.one * 0.5f,
                    moveSpeed * Time.deltaTime
                );

                yield return null;
            }
        }

        GridManager.Instance.OccupancyLayer.SetNode(targetPos, actor);
        actor.MoveRange.DecrementMoveRange(paths.Count);
    }
}
