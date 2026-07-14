using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IUnitRoot))]
public class StatusEffectController : MonoBehaviour
{
    private readonly List<StatusEffectInstance> active = new();

    public void ApplyEffect(StatusEffectDefinition def)
    {
        var existing = active.Find(e => e.definition == def);
        if (existing != null)
            return;

        var instance = new StatusEffectInstance(def);
        active.Add(instance);
        def.behavior.OnApply();
    }

    public void RemoveEffect(StatusEffectInstance instance)
    {
        instance.definition.behavior.OnRemove();
        active.Remove(instance);
    }

    public void TickTurnStart()
    {
        foreach (var instance in active.ToArray()) // ToArray() to avoid InvalidOperationException
        {
            instance.definition.behavior.OnTurnStart();
        }
    }

    public void TickTurnEnd()
    {
        foreach (var instance in active.ToArray()) // ToArray() to avoid InvalidOperationException
        {
            instance.definition.behavior.OnTurnEnd();
            instance.remainingDuration--;
            if (instance.remainingDuration <= 0)
                RemoveEffect(instance);
        }
    }

    public int ModifyIncomingDamage(int damage)
    {
        foreach (var instance in active)
        {
            damage = instance.definition.behavior.ModifyIncomingDamage(damage);
        }

        return damage;
    }

    public IReadOnlyList<StatusEffectInstance> ActiveEffects => active;
}
