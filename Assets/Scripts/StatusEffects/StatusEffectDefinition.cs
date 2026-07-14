using UnityEngine;

[CreateAssetMenu(menuName = "StatusEffects/New Effect")]
public class StatusEffectDefinition : ScriptableObject
{
    public string effectId;
    public string displayName;
    public Sprite icon;

    public int baseDuration;

    public StatusEffectBehaviour behavior;
}
