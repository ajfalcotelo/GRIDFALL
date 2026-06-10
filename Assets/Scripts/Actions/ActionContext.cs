public class ActionContext
{
    public IUnit Actor { get; }
    public PathNode TargetNode { get; }

    public ActionContext(IUnit actor, PathNode targetNode)
    {
        Actor = actor;
        TargetNode = targetNode;
    }
}
