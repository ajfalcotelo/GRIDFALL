using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyUnitRoot))]
public class EnemyUnitController : MonoBehaviour, IUnitController
{
    [SerializeField]
    private BattleManager battleManager;

    [SerializeField]
    private PlayerUnitRoot player;

    public BaseAction SelectedAction { get; set; }
    public PathNode SelectedTargetNode { get; set; }
    public List<PathNode> SelectedPath { get; set; }

    public DecisionState DecisionState { get; private set; }
    public PerformDecisionState PerformDecisionState { get; private set; }
    public EndTurnState EndTurnState { get; private set; }

    public MoveAction MoveAction { get; private set; }
    public AttackAction AttackAction { get; private set; }

    private StateMachine stateMachine;
    private EnemyUnitRoot unit;

    void Awake()
    {
        unit = GetComponent<EnemyUnitRoot>();
    }

    void Start()
    {
        SetupStateMachine();

        transform.position = GridManager.Instance.GetGroundTilemap.GetCellCenterWorld(
            Vector3Int.RoundToInt(transform.position)
        );
        GridManager.Instance.OccupancyLayer.SetNode(transform.position, unit);
    }

    private void SetupStateMachine()
    {
        stateMachine = new();

        DecisionState = new(stateMachine, unit, this, player);
        PerformDecisionState = new(stateMachine, unit, this);
        EndTurnState = new(stateMachine, unit, battleManager);

        MoveAction = new(unit);
        AttackAction = new(unit);
    }

    public void StartTurn()
    {
        stateMachine.SetState(DecisionState);
    }
}
