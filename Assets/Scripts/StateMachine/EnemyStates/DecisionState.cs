using System.Collections.Generic;
using UnityEngine;

public class DecisionState : BaseState
{
    private readonly EnemyUnitController unitController;

    private readonly PlayerUnitRoot player;

    public DecisionState(
        StateMachine stateMachine,
        IUnitRoot unit,
        EnemyUnitController unitController,
        PlayerUnitRoot player
    )
        : base(stateMachine, unit)
    {
        this.unitController = unitController;
        this.player = player;
    }

    public override void Enter()
    {
        var attackNodes = unitController.AttackAction.GetReachableNodes();

        List<PathNode> targets = new();
        foreach (var node in attackNodes)
        {
            if (GridManager.Instance.OccupancyLayer.GetNode(node.Position) != null)
                targets.Add(node);
        }

        var attackTarget = targets.Count > 0 ? GetNearestNodeToPlayer(targets) : null;
        if (attackTarget != null && unit.Stats.ActionCount > 0)
        {
            unitController.SelectedTargetNode = attackTarget;
            unitController.SelectedAction = unitController.AttackAction;
            ChangeState(unitController.PerformDecisionState);
            return;
        }

        var moveNodes = unitController.MoveAction.GetReachableNodes();
        var moveTarget = GetNearestNodeToPlayer(moveNodes);

        if (moveTarget.Position != unit.CurrentNode.Position && unit.MoveRange.CurrentMoveRange > 0)
        {
            unitController.SelectedTargetNode = moveTarget;
            unitController.SelectedAction = unitController.MoveAction;
            unitController.SelectedPath = Pathfinding.FindPath(
                unit.CurrentNode.Position,
                moveTarget.Position
            );
            ChangeState(unitController.PerformDecisionState);
            return;
        }

        ChangeState(unitController.EndTurnState);
    }

    public override void Exit() { }

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
