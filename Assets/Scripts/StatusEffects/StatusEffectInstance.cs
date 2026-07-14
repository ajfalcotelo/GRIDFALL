using System;

[Serializable]
public class StatusEffectInstance
{
    public StatusEffectDefinition definition;
    public int remainingDuration;

    public StatusEffectInstance(StatusEffectDefinition definition)
    {
        this.definition = definition;
        remainingDuration = definition.baseDuration;
    }
}
