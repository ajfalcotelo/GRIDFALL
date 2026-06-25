using UnityEngine;

[RequireComponent(typeof(MoveRange))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(UnitStats))]
[RequireComponent(typeof(PlayerUnitController))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerUnitRoot : MonoBehaviour, IUnitRoot
{
    public UnitStats Stats { get; private set; }
    public Health Health { get; private set; }
    public MoveRange MoveRange { get; private set; }
    public IUnitController Controller { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }

    public PathNode CurrentNode => GridManager.Instance.PathfindLayer.GetNode(transform.position);
    public GameObject GameObject => gameObject;

    void Awake()
    {
        Stats = GetComponent<UnitStats>();
        Health = GetComponent<Health>();
        MoveRange = GetComponent<MoveRange>();
        Controller = GetComponent<PlayerUnitController>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }
}
