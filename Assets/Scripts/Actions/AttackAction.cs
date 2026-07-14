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
        int damage = actor.Stats.Strength;
        damage = context.TargetUnit.StatusEffects.ModifyIncomingDamage(damage);

        actor.Stats.DecrementActionCount();
        context.TargetUnit.Health.TakeDamage(damage);
        Debug.Log(damage);
        yield break;
    }
}
