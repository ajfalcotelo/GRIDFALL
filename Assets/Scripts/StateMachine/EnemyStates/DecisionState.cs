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
        if (attackTarget != null)
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
        unitController.SelectedTargetNode = moveTarget;
        unitController.SelectedAction = unitController.MoveAction;

        ChangeState(unitController.PerformDecisionState);
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
                nearest = node;
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
