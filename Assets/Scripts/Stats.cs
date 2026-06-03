using UnityEngine;

public class Stats : MonoBehaviour
{
    public int CurrentHealth => currentHealth;
    public int Strength => currentStrength;
    public int Defense => currentDefense;
    public int Speed => currentSpeed;

    [SerializeField]
    private UnitData unitData;

    private int currentHealth,
        currentStrength,
        currentDefense,
        currentSpeed;

    void Awake()
    {
        currentHealth = unitData.MaxHealth;
        currentStrength = unitData.Strength;
        currentDefense = unitData.Defense;
        currentSpeed = unitData.Speed;
    }
}
