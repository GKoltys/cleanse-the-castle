using UnityEngine;

[CreateAssetMenu(menuName = "Items/Effects/Thorns")]
public class ThornsRelicEffect : ConsumableEffect
{
    [SerializeField] private float thornPercent = 0f; // % of player max health

    public override void Apply(PlayerApplyEffect player)
    {
        PlayerRelics relics = player.GetComponent<PlayerRelics>();
        if (relics != null)
        {
            relics.AddThorns(thornPercent);
        }
    }

    public override void Remove(PlayerApplyEffect player)
    {
        PlayerRelics relics = player.GetComponent<PlayerRelics>();
        if (relics != null)
        {
            relics.RemoveThorns(thornPercent);
        }
    }

    public override float GetStatChangeValue()
    {
        return thornPercent;
    }
}