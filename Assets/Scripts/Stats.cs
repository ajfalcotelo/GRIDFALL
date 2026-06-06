using UnityEngine;

public class Stats : MonoBehaviour
{
    public int CurrentHealth => currentHealth;
    public int Strength => currentStrength;
    public int Defense => currentDefense;
    public int Speed => currentSpeed;
    public int Range => currentRange;

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
