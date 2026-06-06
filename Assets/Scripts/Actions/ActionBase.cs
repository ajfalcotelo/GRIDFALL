using System.Collections;
using UnityEngine;

public abstract class ActionBase : ScriptableObject
{
    public abstract IEnumerator Execute();
}
