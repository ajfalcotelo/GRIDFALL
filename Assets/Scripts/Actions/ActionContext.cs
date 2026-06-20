public class ActionContext
{
    public IUnitRoot Actor { get; }
    public PathNode TargetNode { get; }

    public ActionContext(IUnitRoot actor, PathNode targetNode)
    {
        Actor = actor;
        TargetNode = targetNode;
    }
}
