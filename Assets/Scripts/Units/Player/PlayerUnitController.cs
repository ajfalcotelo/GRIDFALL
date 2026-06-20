using UnityEngine;

[RequireComponent(typeof(PlayerUnitRoot))]
public class PlayerUnitController : MonoBehaviour, IUnitController
{
    [SerializeField]
    private UIActionMenu actionMenu;

    [SerializeField]
    private TargetingSystem targetingSystem;

    [SerializeField]
    private InputController inputController;

    [SerializeField]
    private BattleManager battleManager;

    public IState CurrentState { get; set; }
    public PathNode SelectedTargetNode { get; set; }
    public BaseAction SelectedAction { get; set; }

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

    void OnEnable()
    {
        inputController.Cancel += OnCancelSelection;
    }

    void OnDisable()
    {
        inputController.Cancel -= OnCancelSelection;
    }

    void Start()
    {
        transform.position = GridManager.Instance.GetGroundTilemap.GetCellCenterWorld(
            Vector3Int.RoundToInt(transform.position)
        );
        GridManager.Instance.GetNode(transform.position).Occupant = unit;
    }

    private void SetupStateMachine()
    {
        StateMachine = new();

        ActionSelectionState = new(StateMachine, unit, this, inputController, actionMenu);
        TargetSelecionState = new(StateMachine, unit, this, inputController, targetingSystem);
        ActionExecutionState = new(StateMachine, unit, this, inputController);
        EndTurnState = new(StateMachine, unit, battleManager);

        MoveAction = new();
        AttackAction = new();
    }

    public void StartTurn()
    {
        StateMachine.SetState(ActionSelectionState);
    }

    private void OnCancelSelection()
    {
        StateMachine.ChangeState(ActionSelectionState);
        targetingSystem.ClearSetTiles();
    }
}
