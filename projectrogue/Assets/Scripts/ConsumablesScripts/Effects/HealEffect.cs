using UnityEngine;

[CreateAssetMenu(menuName = "Items/Effects/Heal")]
public class HealEffect : ConsumableEffect
{
    [SerializeField] private float healAmount;

    public override void Apply(PlayerApplyEffect player)
    {
        player.Heal(healAmount);
    }
}
