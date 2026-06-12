using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    private readonly float moveSpeed = 5f;

    protected override IEnumerator Execute(ActionContext context)
    {
        GridManager.Instance.GetNode(context.Actor.GridPosition).Occupant = null;
        List<PathNode> paths = Pathfinding.FindPath(
            context.Actor.GridPosition,
            context.TargetNode.Position
        );
        if (paths == null)
            yield break;

        foreach (PathNode path in paths)
        {
            while (
                Vector2.Distance(
                    context.Actor.GameObject.transform.position,
                    GridManager.Instance.XYToWorldPos(path.Position.x, path.Position.y)
                        + Vector2.one * 0.5f
                ) > 0.05f
            )
            {
                context.Actor.GameObject.transform.position = Vector2.MoveTowards(
                    context.Actor.GameObject.transform.position,
                    GridManager.Instance.XYToWorldPos(path.Position.x, path.Position.y)
                        + Vector2.one * 0.5f,
                    moveSpeed * Time.deltaTime
                );

                yield return null;
            }
        }

        context.TargetNode.Occupant = context.Actor;
    }
}
