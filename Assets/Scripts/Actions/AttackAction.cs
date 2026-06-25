using System.Collections;

public class AttackAction : BaseAction
{
    public override bool CanRun(ActionContext context)
    {
        return context.TargetUnit != null;
    }

    public override TargetingData GetTargetingData(IUnitRoot unit)
    {
        return new TargetingData(unit.Stats.Range, ActionType.Attack);
    }

    protected override IEnumerator Execute(ActionContext context)
    {
        context.Actor.Stats.DecrementActionCount();
        context.TargetUnit.Health.TakeDamage(context.Actor.Stats.Strength);
        yield break;
    }
}
