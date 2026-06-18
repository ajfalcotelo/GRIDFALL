using UnityEngine;

[RequireComponent(typeof(EnemyUnitRoot))]
public class EnemyUnitController : MonoBehaviour, IUnitController
{
    public StateMachine StateMachine { get; }

    private EnemyUnitRoot unit;

    void Awake()
    {
        unit = GetComponent<EnemyUnitRoot>();
    }

    void Start()
    {
        GridManager.Instance.GetNode(transform.position).Occupant = unit;
        transform.position = GridManager.Instance.GetGroundTilemap.GetCellCenterWorld(
            Vector3Int.RoundToInt(transform.position)
        );
    }
}
