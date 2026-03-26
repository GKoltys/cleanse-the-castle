using UnityEngine;

[CreateAssetMenu(fileName = "Lifesteal", menuName = "Items/Effects/Lifesteal")]
public class LifeStealRelicEffect : ConsumableEffect
{
    [SerializeField] private float lifeStealPercent;

    public override void Apply(PlayerApplyEffect player)
    {
        Debug.Log("LifeStealRelicEffect.Apply called");
        PlayerRelics relics = player.GetComponent<PlayerRelics>();
        if (relics != null)
        {
            relics.AddLifeSteal(lifeStealPercent);
        }
    }

    public override void Remove(PlayerApplyEffect player)
    {
        PlayerRelics relics = player.GetComponent<PlayerRelics>();
        if (relics != null)
        {
            relics.RemoveLifeSteal(lifeStealPercent);
        }
    }

    public override float GetStatChangeValue()
    {
        return lifeStealPercent;
    }
}