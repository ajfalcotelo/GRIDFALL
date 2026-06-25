using System.Collections.Generic;
using UnityEngine;

public class DecisionState : BaseState
{
    private readonly EnemyUnitController unitController;
    private readonly TargetingSystem targetingSystem;
    private readonly PlayerUnitRoot player;

    public DecisionState(
        StateMachine stateMachine,
        IUnitRoot unit,
        EnemyUnitController unitController,
        TargetingSystem targetingSystem,
        PlayerUnitRoot player
    )
        : base(stateMachine, unit)
    {
        this.unitController = unitController;
        this.targetingSystem = targetingSystem;
        this.player = player;
    }

    public override void Enter()
    {
        var nodes = targetingSystem.GetReachableNodes(
            unit,
            unitController.AttackAction.GetTargetingData(unit)
        );

        List<PathNode> targets = new();
        foreach (var node in nodes)
        {
            if (node.Occupant != null)
                targets.Add(node);
        }

        var attackTarget =
            targets.Count > 0 ? GetNearestNode(targets, unit.CurrentNode.Position) : null;
        if (attackTarget != null && unit.Stats.ActionCount > 0)
        {
            unitController.SelectedTargetNode = attackTarget;
            unitController.SelectedAction = unitController.AttackAction;
            ChangeState(unitController.PerformDecisionState);
            return;
        }

        nodes = targetingSystem.GetReachableNodes(
            unit,
            unitController.MoveAction.GetTargetingData(unit)
        );
        var moveTarget = GetNearestNode(nodes, player.CurrentNode.Position);
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

    private PathNode GetNearestNode(List<PathNode> nodes, Vector2Int target)
    {
        PathNode nearest = nodes[0];
        int minDist = GetDistance(nearest.Position, target);

        foreach (var node in nodes)
        {
            var dist = GetDistance(node.Position, target);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = node;
            }
        }

        return nearest;
    }

    private int GetDistance(Vector2Int node, Vector2Int target)
    {
        var dx = Mathf.Abs(node.x - target.x);
        var dy = Mathf.Abs(node.y - target.y);
        return Mathf.Max(dx, dy);
    }
}
