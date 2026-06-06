using UnityEngine;

public enum PlayerAction
{
    Move,
}

public enum PlayerState
{
    SelectAction,
    SelectTarget,
    PerformAction,
    EndTurn,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private InputController inputController;

    [SerializeField]
    private UIActionMenu actionMenu;

    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private GameObject enemyPrefab;

    public GameObject CurrentUnit { get; private set; }

    private PlayerAction currentAction;
    private TargetingSystem currentTarget;
    private PathNode selectedNode;

    public void SetCurrentAction(PlayerAction action) => currentAction = action;

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        inputController.SelectedNode += GetSelectedNode;
    }

    void OnDisable()
    {
        inputController.SelectedNode -= GetSelectedNode;
    }

    void Start()
    {
        CurrentUnit = SpawnUnit(playerPrefab, new Vector2Int(0, 0));
        SpawnUnit(enemyPrefab, new Vector2Int(0, 5));
        SpawnUnit(enemyPrefab, new Vector2Int(0, 7));
    }

    public void SetPlayerState(PlayerState state)
    {
        inputController.Deactivate();

        switch (state)
        {
            case PlayerState.SelectAction:
                actionMenu.SetActionMenuActive();
                break;
            case PlayerState.SelectTarget:
                inputController.Activate();
                break;
            case PlayerState.PerformAction:
                switch (currentAction)
                {
                    case PlayerAction.Move:
                        TargetingSystem targetingSystem = new();
                        var targetNode = targetingSystem.ValidateSelectedNode(selectedNode);
                        if (targetNode != null)
                        {
                            MoveAction moveAction = new(targetNode);
                            PerformAction(moveAction);
                        }
                        break;
                    default:
                        break;
                }
                break;
            case PlayerState.EndTurn:
                actionMenu.SetActionMenuActive();
                break;
        }
    }

    public void PerformAction(ActionBase action)
    {
        StartCoroutine(action.Execute());
        SetPlayerState(PlayerState.EndTurn);
    }

    private void GetSelectedNode(PathNode node)
    {
        selectedNode = node;
    }

    private GameObject SpawnUnit(GameObject prefab, Vector2Int cellposition)
    {
        GameObject unit = Instantiate(prefab);
        Vector2 pos = GridManager.Instance.XYToWorldPos(cellposition.x, cellposition.y);
        unit.transform.position = GridManager.Instance.GetGroundTilemap.GetCellCenterWorld(
            Vector3Int.RoundToInt(pos)
        );
        PathNode entityNode = GridManager.Instance.GetNode(unit.transform.position);
        entityNode.Occupant = unit;

        return unit;
    }
}
