using System.Collections.Generic;

public class ActionContext
{
    public IUnitRoot Actor { get; set; }
    public IUnitRoot TargetUnit { get; set; }
    public PathNode TargetNode { get; set; }
    public List<PathNode> Path { get; set; }
}
