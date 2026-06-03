using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private UIActionMenu actionMenu;

    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private GameObject enemyPrefab;

    public PlayerController PlayerController { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        PlayerController = SpawnUnit(playerPrefab, new Vector2Int(0, 0), true)
            .GetComponent<PlayerController>();
        PlayerController.OnActionFinished += EndTurn;

        SpawnUnit(enemyPrefab, new Vector2Int(0, 5));
        SpawnUnit(enemyPrefab, new Vector2Int(0, 7));
    }

    private void EndTurn()
    {
        actionMenu.SetActionMenuActive();
    }

    private GameObject SpawnUnit(
        GameObject prefab,
        Vector2Int cellposition,
        bool isPlayerControllable = false
    )
    {
        GameObject unit = Instantiate(prefab);
        unit.transform.position = GridManager.Instance.XYToWorldPos(cellposition.x, cellposition.y);
        PathNode entityNode = GridManager.Instance.GetNode(unit.transform.position);
        entityNode.Occupant = unit;

        if (isPlayerControllable)
            return unit;

        return null;
    }
}
