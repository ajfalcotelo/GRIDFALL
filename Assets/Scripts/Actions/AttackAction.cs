using System.Collections;

public class AttackAction : BaseAction
{
    public override bool CanRun(ActionContext context)
    {
        return context.TargetNode.Occupant != null;
    }

    protected override IEnumerator Execute(ActionContext context)
    {
        if (!context.TargetNode.Occupant.GameObject.TryGetComponent<Health>(out var targetHealth))
            yield break;

        targetHealth.TakeDamage(context.Actor.Strength);
    }
}
