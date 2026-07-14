using UnityEngine;

[CreateAssetMenu(menuName = "StatusEffects/Behaviours/Defend")]
public class DefendBehaviour : StatusEffectBehaviour
{
    public override int ModifyIncomingDamage(int damage)
    {
        return damage /= 2;
    }
}
