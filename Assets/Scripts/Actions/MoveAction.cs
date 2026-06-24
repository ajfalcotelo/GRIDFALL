using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    private readonly float moveSpeed = 5f;

    public override bool CanRun(ActionContext context)
    {
        return context.TargetNode != null;
    }

    public override TargetingData GetTargetingData(IUnitRoot unit)
    {
        return new TargetingData(unit.MoveRange.CurrentMoveRange, ActionType.Move);
    }

    protected override IEnumerator Execute(ActionContext context)
    {
        context.Actor.CurrentNode.Occupant = null;
        var sourcePos = context.Actor.CurrentNode.Position;
        var targetPos = context.TargetNode.Position;
        List<PathNode> paths = Pathfinding.FindPath(sourcePos, targetPos);
        if (paths == null)
            yield break;

        foreach (PathNode path in paths)
        {
            while (
                Vector2.Distance(
                    context.Actor.GameObject.transform.position,
                    GridManager.Instance.XYToWorldPos(path.Position) + Vector2.one * 0.5f
                ) > 0.05f
            )
            {
                context.Actor.GameObject.transform.position = Vector2.MoveTowards(
                    context.Actor.GameObject.transform.position,
                    GridManager.Instance.XYToWorldPos(path.Position) + Vector2.one * 0.5f,
                    moveSpeed * Time.deltaTime
                );

                yield return null;
            }
        }

        context.TargetNode.Occupant = context.Actor;
        context.Actor.MoveRange.DecrementMoveRange(GetDistance(sourcePos, targetPos));
    }

    private int GetDistance(Vector2Int a, Vector2Int b)
    {
        var dx = Mathf.Abs(a.x - b.x);
        var dy = Mathf.Abs(a.y - b.y);
        return dx + dy;
    }
}
