using UnityEngine;

[RequireComponent(typeof(UnitStats))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(MoveRange))]
[RequireComponent(typeof(EnemyUnitController))]
public class EnemyUnitRoot : MonoBehaviour, IUnitRoot
{
    public Health Health { get; private set; }
    public MoveRange MoveRange { get; private set; }
    public UnitStats Stats { get; private set; }
    public IUnitController Controller { get; private set; }
    public Vector2Int GridPosition => GridManager.Instance.WorldToXY(transform.position);

    void Awake()
    {
        Stats = GetComponent<UnitStats>();
        Health = GetComponent<Health>();
        MoveRange = GetComponent<MoveRange>();
        Controller = GetComponent<EnemyUnitController>();
    }
}
