using UnityEngine;

[CreateAssetMenu(menuName = "Items/Effects/Speed")]
public class SpeedEffect : ConsumableEffect
{
    [SerializeField] private float speedBoost;

    public override void Apply(PlayerApplyEffect player)
    {
        player.ChangePlayerSpeed(speedBoost);
    }

    public override float GetStatChangeValue()
    {
        return speedBoost;
    }
}
