using UnityEngine;

public class PathNode
{
    public int X {get;}
    public int Y {get;}
    public PathNode Parent {get; private set;}
    public int GCost {get; private set;}
    public int HCost {get; private set;}
    public int FCost => GCost + HCost;

    public PathNode(int X, int Y)
    {
        this.X = X;
        this.Y = Y;
    }

    public PathNode SetParent(PathNode pathNode) => Parent = pathNode;
    public void SetGCost(int value) => GCost = value;
    public void SetHCost(int value) => HCost = value;

    public override string ToString()
    {
        return new Vector2Int(X, Y).ToString();
    }

}
