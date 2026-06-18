using UnityEngine;

public interface IUnitRoot
{
    Health Health { get; }
    MoveRange MoveRange { get; }
    UnitStats Stats { get; }
    IUnitController Controller { get; }
    Vector2Int GridPosition { get; }
}
