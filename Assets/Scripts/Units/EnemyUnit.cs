using UnityEngine;

public class EnemyUnit : MonoBehaviour, IUnit
{
    public int CurrentHealth => currentHealth;
    public int Strength => currentStrength;
    public int Defense => currentDefense;
    public int Speed => currentSpeed;
    public int Range => currentRange;

    public Vector2 UnitGridPosition => GridManager.Instance.WorldToXY(transform.position);

    [SerializeField]
    private UnitData unitData;

    private int currentHealth,
        currentStrength,
        currentDefense,
        currentSpeed,
        currentRange;

    void Awake()
    {
        currentHealth = unitData.MaxHealth;
        currentStrength = unitData.Strength;
        currentDefense = unitData.Defense;
        currentSpeed = unitData.Speed;
        currentRange = unitData.Range;
    }
}
