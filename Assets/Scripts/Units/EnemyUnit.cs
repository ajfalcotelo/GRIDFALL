using UnityEngine;

public class EnemyUnit : MonoBehaviour, IUnit
{
    public int CurrentHealth => currentHealth;
    public int Strength => currentStrength;
    public int Defense => currentDefense;
    public int Speed => currentSpeed;
    public int Range => currentRange;

    public Vector2Int GridPosition => GridManager.Instance.WorldToXY(transform.position);
    public GameObject GameObject => gameObject;

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

    // void Start()
    // {
    //     GridManager.Instance.GetNode(transform.position).Occupant = this;
    // }
}
