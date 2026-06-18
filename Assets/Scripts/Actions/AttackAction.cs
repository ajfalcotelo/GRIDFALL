using System.Collections;

public class AttackAction : BaseAction
{
    public override bool CanRun(ActionContext context)
    {
        return context.TargetNode.Occupant != null;
    }

    public override TargetingData GetTargetingData(IUnit unit)
    {
        return new TargetingData(unit.Range);
    }

    protected override IEnumerator Execute(ActionContext context)
    {
        context.TargetNode.Occupant.Health.TakeDamage(context.Actor.Strength);
        yield break;
    }
}
