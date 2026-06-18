public class ActionContext
{
    public PlayerUnitRoot Actor { get; }
    public PathNode TargetNode { get; }

    public ActionContext(PlayerUnitRoot actor, PathNode targetNode)
    {
        Actor = actor;
        TargetNode = targetNode;
    }
}
