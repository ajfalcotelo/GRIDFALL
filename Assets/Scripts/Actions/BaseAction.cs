using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction
{
    private Action OnActionFinished = delegate { };
    protected IUnitRoot actor;

    public BaseAction(IUnitRoot unit)
    {
        actor = unit;
    }

    public Coroutine Run(MonoBehaviour owner, ActionContext context, Action onActionFinished)
    {
        OnActionFinished = onActionFinished;
        return owner.StartCoroutine(RunInternal(context));
    }

    private IEnumerator RunInternal(ActionContext context)
    {
        yield return Execute(context);

        OnActionFinished.Invoke();
    }

    public abstract List<PathNode> GetReachableNodes();
    public abstract bool CanRun(ActionContext context);
    protected abstract IEnumerator Execute(ActionContext context);
}
