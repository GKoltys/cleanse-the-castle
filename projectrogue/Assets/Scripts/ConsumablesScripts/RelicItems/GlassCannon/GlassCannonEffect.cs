using UnityEngine;

[CreateAssetMenu(fileName = "GlassCannon", menuName = "Items/Effects/GlassCannon")]
public class GlassCannonRelicEffect : ConsumableEffect
{
    [SerializeField] private float damageBonus;
    [SerializeField] private float extraDamageTaken;

    public override void Apply(PlayerApplyEffect player)
    {
        player.ChangePlayerDamageMultiplier(damageBonus);
        player.ChangePlayerDamageTakenMultiplier(extraDamageTaken);
    }

    public override void Remove(PlayerApplyEffect player)
    {
        player.ChangePlayerDamageMultiplier(-damageBonus);
        player.ChangePlayerDamageTakenMultiplier(-extraDamageTaken);
    }

    public override float GetStatChangeValue()
    {
        return damageBonus;
    }
}