using UnityEngine;

public interface IUnit
{
    Health Health { get; }
    MoveRange MoveRange { get; }
    int Strength { get; }
    int Defense { get; }
    int Speed { get; }
    int Range { get; }

    Vector2Int GridPosition { get; }
    GameObject GameObject { get; }
}
