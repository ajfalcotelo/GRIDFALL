using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : BaseAction
{
    public AttackAction(IUnitRoot unit)
        : base(unit) { }

    public override bool CanRun(ActionContext context)
    {
        return context.TargetUnit != null;
    }

    public override float Score(ActionContext context)
    {
        if (context.TargetUnit == null)
            return 0;

        if (actor.Stats.ActionCount > 0)
            return 2f;

        return 0;
    }

    public override List<PathNode> GetReachableNodes()
    {
        Vector2Int sourcePosition = actor.CurrentNode.Position;
        List<PathNode> inRangeNodes = new();
        var range = actor.Stats.Range;

        for (int x = sourcePosition.x - range; x <= sourcePosition.x + range; x++)
        {
            for (int y = sourcePosition.y - range; y <= sourcePosition.y + range; y++)
            {
                var pos = new Vector2Int(x, y);
                var node = GridManager.Instance.PathfindLayer.GetNode(pos);
                if (node == null || sourcePosition == pos)
                    continue;
                inRangeNodes.Add(node);
            }
        }

        return inRangeNodes;
    }

    protected override IEnumerator Execute(ActionContext context)
    {
        actor.Stats.DecrementActionCount();
        context.TargetUnit.Health.TakeDamage(actor.Stats.Strength);
        yield break;
    }
}
