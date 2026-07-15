using System;
using UnityEngine;

[Serializable]
public struct StatusEffectApplication
{
    public StatusEffectDefinition effect;

    [Range(0f, 1f)]
    public float chance;
}
