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

    public event Action OnActionExecuted = delegate { };

    public IState CurrentState { get; set; }
    public PathNode SelectedTargetNode { get; set; }
    public IAction SelectedAction { get; set; }

    public ActionSelectionState ActionSelectionState { get; private set; }
    public TargetSelectionState TargetSelecionState { get; private set; }
    public ActionExecutionState ActionExecutionState { get; private set; }

    public MoveAction MoveAction { get; private set; }

    void Start()
    {
        ActionSelectionState = new(this, actionMenu);
        TargetSelecionState = new(this, currentUnit, inputController, targetingSystem);
        ActionExecutionState = new(this, currentUnit);

        MoveAction = new();

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

    public void ExecuteAction(ActionContext context)
    {
        StartCoroutine(SelectedAction.Execute(context));
        OnActionExecuted.Invoke();
    }
}
