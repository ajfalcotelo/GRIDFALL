using System.Collections;
using UnityEngine;

public class AttackAction : BaseAction
{
    protected override IEnumerator Execute(ActionContext context)
    {
        if (!context.TargetNode.Occupant.GameObject.TryGetComponent<Health>(out var targetHealth))
            yield break;

        targetHealth.TakeDamage(context.Actor.Strength);
    }
}
