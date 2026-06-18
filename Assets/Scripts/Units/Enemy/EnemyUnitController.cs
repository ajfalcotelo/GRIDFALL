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
        transform.position = GridManager.Instance.GetGroundTilemap.GetCellCenterWorld(
            Vector3Int.RoundToInt(transform.position)
        );
        GridManager.Instance.GetNode(transform.position).Occupant = unit;
    }
}
