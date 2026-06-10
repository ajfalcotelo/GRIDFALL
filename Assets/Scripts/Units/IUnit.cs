using UnityEngine;

public interface IUnit
{
    int CurrentHealth { get; }
    int Strength { get; }
    int Defense { get; }
    int Speed { get; }
    int Range { get; }

    Vector2 UnitGridPosition { get; }
}
