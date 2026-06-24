using UnityEngine;

public interface IUnitRoot
{
    Health Health { get; }
    MoveRange MoveRange { get; }
    UnitStats Stats { get; }
    IUnitController Controller { get; }
    PathNode CurrentNode { get; }
    GameObject GameObject { get; }
    SpriteRenderer SpriteRenderer { get; }
}
