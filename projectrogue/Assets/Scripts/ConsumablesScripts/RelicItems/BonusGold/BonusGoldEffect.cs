using UnityEngine;

[CreateAssetMenu(fileName = "BonusGold", menuName = "Items/Effects/BonusGold")]
public class BonusGold : ConsumableEffect
{
    [SerializeField] private int bonusGold;

    public override void Apply(PlayerApplyEffect player)
    {
        PlayerRelics relics = player.GetComponent<PlayerRelics>();
        if (relics != null)
        {
            relics.AddBonusGold(bonusGold);
        }
    }

    public override void Remove(PlayerApplyEffect player)
    {
        PlayerRelics relics = player.GetComponent<PlayerRelics>();
        if (relics != null)
        {
            relics.RemoveBonusGold(bonusGold);
        }
    }

    public override float GetStatChangeValue()
    {
        return bonusGold;
    }
}