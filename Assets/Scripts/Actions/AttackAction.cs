using System.Collections;

public class AttackAction : BaseAction
{
    public override bool CanRun(ActionContext context)
    {
        return context.TargetNode.Occupant != null;
    }

    protected override IEnumerator Execute(ActionContext context)
    {
        context.TargetNode.Occupant.Health.TakeDamage(context.Actor.Strength);
        yield break;
    }
}
