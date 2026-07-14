using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(MoveRange))]
[RequireComponent(typeof(UnitStats))]
[RequireComponent(typeof(EnemyUnitController))]
[RequireComponent(typeof(StatusEffectController))]
public class EnemyUnitRoot : MonoBehaviour, IUnitRoot
{
    public SpriteRenderer SpriteRenderer { get; private set; }
    public Health Health { get; private set; }
    public MoveRange MoveRange { get; private set; }
    public UnitStats Stats { get; private set; }
    public IUnitController Controller { get; private set; }
    public StatusEffectController StatusEffects { get; private set; }

    public PathNode CurrentNode => GridManager.Instance.PathfindLayer.GetNode(transform.position);
    public GameObject GameObject => gameObject;

    void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Health = GetComponent<Health>();
        MoveRange = GetComponent<MoveRange>();
        Stats = GetComponent<UnitStats>();
        Controller = GetComponent<EnemyUnitController>();
        StatusEffects = GetComponent<StatusEffectController>();
    }
}
