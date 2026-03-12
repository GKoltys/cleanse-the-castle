using UnityEngine;

[CreateAssetMenu(menuName = "Items/Effects/Speed")]
public class SpeedBoostEffect : ConsumableEffect
{
    [SerializeField] private float speedBoost;

    public override void Apply(PlayerApplyEffect player)
    {
        player.AddPermanentSpeedBoost(speedBoost);
    }
}
