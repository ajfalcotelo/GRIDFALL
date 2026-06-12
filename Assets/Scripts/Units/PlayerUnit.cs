using UnityEngine;

[RequireComponent(typeof(Health))]
public class PlayerUnit : MonoBehaviour, IUnit
{
    [SerializeField]
    private UnitData unitData;

    public int CurrentHealth => health.CurrentHealth;
    public int Strength => currentStrength;
    public int Defense => currentDefense;
    public int Speed => currentSpeed;
    public int Range => currentRange;

    public Vector2Int GridPosition => GridManager.Instance.WorldToXY(transform.position);
    public GameObject GameObject => gameObject;

    private Health health;
    private int currentStrength,
        currentDefense,
        currentSpeed,
        currentRange;

    void Awake()
    {
        health = GetComponent<Health>();
        currentStrength = unitData.Strength;
        currentDefense = unitData.Defense;
        currentSpeed = unitData.Speed;
        currentRange = unitData.Range;
    }

    void Start()
    {
        GridManager.Instance.GetNode(transform.position).Occupant = this;
        transform.position = GridManager.Instance.GetGroundTilemap.GetCellCenterWorld(
            Vector3Int.RoundToInt(transform.position)
        );
    }
}
