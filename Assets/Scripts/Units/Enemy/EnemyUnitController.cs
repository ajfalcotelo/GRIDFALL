using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyUnitRoot))]
public class EnemyUnitController : MonoBehaviour, IUnitController
{
    [SerializeField]
    private BattleManager battleManager;

    [SerializeField]
    private PlayerUnitRoot player;

    public MoveAction MoveAction { get; private set; }
    public AttackAction AttackAction { get; private set; }

    private EnemyUnitRoot unit;

    void Awake()
    {
        unit = GetComponent<EnemyUnitRoot>();
    }

    void Start()
    {
        MoveAction = new(unit);
        AttackAction = new(unit);

        transform.position = GridManager.Instance.GetGroundTilemap.GetCellCenterWorld(
            Vector3Int.RoundToInt(transform.position)
        );
        GridManager.Instance.OccupancyLayer.SetNode(transform.position, unit);
    }

    private void EvaluateActions(ActionContext context)
    {
        List<BaseAction> actions = new() { MoveAction, AttackAction };

        BaseAction bestAction = null;
        float bestScore = float.MinValue;

        foreach (var action in actions)
        {
            var score = action.Score(context);
            if (score > bestScore)
            {
                bestAction = action;
                bestScore = score;
            }
        }

        bestAction?.Run(this, context, EndTurn);
    }

    public void StartTurn()
    {
        var attackNodes = AttackAction.GetReachableNodes();
        List<PathNode> targets = new();
        foreach (var node in attackNodes)
        {
            if (GridManager.Instance.OccupancyLayer.GetNode(node.Position) != null)
                targets.Add(node);
        }
        var attackTarget = targets.Count > 0 ? GetNearestNodeToPlayer(targets) : null;
        var attackUnitTarget =
            attackTarget == null
                ? null
                : GridManager.Instance.OccupancyLayer.GetNode(attackTarget.Position);

        var moveNodes = MoveAction.GetReachableNodes();
        var moveTarget = GetNearestNodeToPlayer(moveNodes);

        EvaluateActions(
            new ActionContext()
            {
                TargetUnit = attackUnitTarget,
                TargetNode = moveTarget,
                Path = Pathfinding.FindPath(unit.CurrentNode.Position, moveTarget.Position),
            }
        );
    }

    private void EndTurn()
    {
        unit.MoveRange.ResetMoveRange();
        unit.Stats.ResetActionCount();
        battleManager.NextUnitTurn();
    }

    private PathNode GetNearestNodeToPlayer(List<PathNode> nodes)
    {
        PathNode nearest = nodes[0];
        int minDist = GetDistance(nearest.Position, player.CurrentNode.Position);
        int minSteps = GetStepsToPlayer(nearest);

        foreach (var node in nodes)
        {
            var steps = GetStepsToPlayer(node);

            if (steps < minSteps)
            {
                minSteps = steps;
                minDist = GetDistance(nearest.Position, player.CurrentNode.Position);
                nearest = node;
            }
            else if (steps == minSteps)
            {
                var dist = GetDistance(node.Position, player.CurrentNode.Position);
                if (dist < minDist)
                    nearest = node;
            }
        }

        return nearest;
    }

    private int GetDistance(Vector2Int a, Vector2Int b)
    {
        var dx = Mathf.Abs(a.x - b.x);
        var dy = Mathf.Abs(a.y - b.y);
        return dx + dy;
    }

    private int GetStepsToPlayer(PathNode node)
    {
        List<PathNode> path = Pathfinding.FindPath(node.Position, player.CurrentNode.Position);
        if (path == null)
            return -1;

        return path.Count;
    }
}
