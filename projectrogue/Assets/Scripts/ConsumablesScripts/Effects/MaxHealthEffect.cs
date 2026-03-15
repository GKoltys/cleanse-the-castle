using UnityEngine;

[CreateAssetMenu(menuName = "Items/Effects/MaxHealth")]
public class MaxHeathEffect : ConsumableEffect
{
    [SerializeField] private float maxHealthChange;

    public override void Apply(PlayerApplyEffect player)
    {
        player.ChangePlayerMaxHealth(maxHealthChange);
    }

    public override float GetStatChangeValue()
    {
        return maxHealthChange;
    }
}
