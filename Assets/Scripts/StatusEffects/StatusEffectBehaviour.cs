using UnityEngine;

public abstract class StatusEffectBehaviour : ScriptableObject
{
    public virtual void OnApply() { }

    public virtual void OnTurnStart() { }

    public virtual void OnTurnEnd() { }

    public virtual void OnRemove() { }

    public virtual int ModifyIncomingDamage(int damage) => damage;
}
