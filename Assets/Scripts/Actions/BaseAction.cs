using System;
using System.Collections;
using UnityEngine;

public abstract class BaseAction
{
    private Action OnActionFinished = delegate { };

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

    protected abstract IEnumerator Execute(ActionContext context);
}
