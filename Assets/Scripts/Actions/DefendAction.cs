using System.Collections;
using System.Collections.Generic;

public class DefendAction : BaseAction
{
    public AbilityDefinition defendAbility;

    public DefendAction(IUnitRoot unit)
        : base(unit) { }

    public override bool CanRun(ActionContext context)
    {
        return true; //temp
    }

    public override List<PathNode> GetReachableNodes()
    {
        return new List<PathNode>() { actor.CurrentNode };
    }

    protected override IEnumerator Execute(ActionContext context)
    {
        actor.Controller.UseAbility();
        yield break;
    }
}
