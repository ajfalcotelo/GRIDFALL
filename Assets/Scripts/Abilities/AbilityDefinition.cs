using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/New Ability")]
public class AbilityDefinition : ScriptableObject
{
    public string abilityName;

    public List<StatusEffectApplication> effectsToApply;
}
