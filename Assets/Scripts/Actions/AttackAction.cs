using System.Collections;

public class AttackAction : BaseAction
{
    public override bool CanRun(ActionContext context)
    {
        return context.TargetNode.Occupant != null;
    }

    public override TargetingData GetTargetingData(PlayerUnitRoot unit)
    {
        return new TargetingData(unit.Stats.Range);
    }

    protected override IEnumerator Execute(ActionContext context)
    {
        context.TargetNode.Occupant.Health.TakeDamage(context.Actor.Stats.Strength);
        yield break;
    }
}
