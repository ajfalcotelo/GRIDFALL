using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerUnitRoot))]
public class PlayerUnitController : MonoBehaviour, IUnitController
{
    [SerializeField]
    private UIActionMenu actionMenu;

    [SerializeField]
    private InputController inputController;

    [SerializeField]
    private NodeHighlighter nodeHighlighter;

    [SerializeField]
    private MovementPreviewSystem pathLineRenderer;

    [SerializeField]
    private BattleManager battleManager;

    public PathNode SelectedTargetNode { get; set; }
    public BaseAction SelectedAction { get; set; }
    public List<PathNode> SelectedPath { get; set; }

    public ActionSelectionState ActionSelectionState { get; private set; }
    public TargetSelectionState TargetSelecionState { get; private set; }
    public ActionExecutionState ActionExecutionState { get; private set; }
    public EndTurnState EndTurnState { get; private set; }

    public MoveAction MoveAction { get; private set; }
    public AttackAction AttackAction { get; private set; }

    public StateMachine StateMachine { get; private set; }

    private PlayerUnitRoot unit;

    void Awake()
    {
        unit = GetComponent<PlayerUnitRoot>();
        SetupStateMachine();
    }

    void Start()
    {
        transform.position = GridManager.Instance.GetGroundTilemap.GetCellCenterWorld(
            Vector3Int.RoundToInt(transform.position)
        );
        GridManager.Instance.OccupancyLayer.SetNode(transform.position, unit);
    }

    private void SetupStateMachine()
    {
        StateMachine = new();

        ActionSelectionState = new(StateMachine, unit, this, inputController, actionMenu);
        TargetSelecionState = new(
            StateMachine,
            unit,
            this,
            inputController,
            nodeHighlighter,
            pathLineRenderer
        );
        ActionExecutionState = new(StateMachine, unit, this, inputController);
        EndTurnState = new(StateMachine, unit, battleManager);

        MoveAction = new(unit);
        AttackAction = new(unit);
    }

    public void StartTurn()
    {
        StateMachine.SetState(ActionSelectionState);
    }
}
