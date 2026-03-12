using UnityEngine;

[CreateAssetMenu(menuName = "Items/Effects/MaxHealthIncrease")]
public class MaxHeathEffect : ConsumableEffect
{
    [SerializeField] private float maxHealthIncrease;

    public override void Apply(PlayerApplyEffect player)
    {
        player.ChangePlayerMaxHealth(maxHealthIncrease);
    }
}
