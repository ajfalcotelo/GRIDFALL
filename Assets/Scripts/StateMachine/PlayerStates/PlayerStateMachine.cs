using System;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField]
    private UIActionMenu actionMenu;

    [SerializeField]
    private TargetingSystem targetingSystem;

    [SerializeField]
    private InputController inputController;

    [SerializeField]
    private PlayerUnit currentUnit; // temp, should have a list of units controllable by player

    public Action OnActionExecuted { get; set; }
    public IState CurrentState { get; set; }
    public PathNode SelectedTargetNode { get; set; }
    public BaseAction SelectedAction { get; set; }

    public ActionSelectionState ActionSelectionState { get; private set; }
    public TargetSelectionState TargetSelecionState { get; private set; }
    public ActionExecutionState ActionExecutionState { get; private set; }

    public MoveAction MoveAction { get; private set; }
    public AttackAction AttackAction { get; private set; }

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
        ActionSelectionState = new(this, actionMenu, inputController);
        TargetSelecionState = new(this, currentUnit, inputController, targetingSystem);
        ActionExecutionState = new(this, currentUnit, inputController);

        MoveAction = new();
        AttackAction = new();

        SetState(ActionSelectionState);
    }

    public void SetState(IState state)
    {
        CurrentState = state;
        CurrentState.Enter();
    }

    public void ChangeState(IState state)
    {
        CurrentState.Exit();
        CurrentState = state;
        CurrentState.Enter();
    }

    private void OnCancelSelection()
    {
        ChangeState(ActionSelectionState);
        targetingSystem.ClearSetTiles();
    }
}
