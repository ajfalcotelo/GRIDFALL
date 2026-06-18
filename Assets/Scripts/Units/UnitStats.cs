using UnityEngine;

public class UnitStats : MonoBehaviour
{
    [SerializeField]
    private UnitData unitData;

    public int Strength => currentStrength;
    public int Defense => currentDefense;
    public int Speed => currentSpeed;
    public int Range => currentRange;

    public int ActionCount { get; private set; }

    private int currentStrength,
        currentDefense,
        currentSpeed,
        currentRange;

    void Awake()
    {
        currentStrength = unitData.Strength;
        currentDefense = unitData.Defense;
        currentSpeed = unitData.Speed;
        currentRange = unitData.Range;
        ActionCount = 1;
    }

    public void ResetActionCount() => ActionCount = 1;

    public void DecrementActionCount() => --ActionCount;
}
