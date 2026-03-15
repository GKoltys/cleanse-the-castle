using UnityEngine;

[CreateAssetMenu(menuName = "Items/Effects/Damage")]
public class DamageEffect : ConsumableEffect
{
    [SerializeField] private float damageMultiplierChange;

    public override void Apply(PlayerApplyEffect player)
    {
        player.ChangePlayerDamageMultiplier(damageMultiplierChange);
    }

    public override float GetStatChangeValue()
    {
        return damageMultiplierChange;
    }
}
