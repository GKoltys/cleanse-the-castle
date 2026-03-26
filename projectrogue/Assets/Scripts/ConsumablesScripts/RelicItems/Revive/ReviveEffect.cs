using UnityEngine;

[CreateAssetMenu(fileName = "Revive", menuName = "Items/Effects/Revive")]
public class ReviveEffect : ConsumableEffect
{
    [SerializeField] private float reviveHealthPercent;

    public override void Apply(PlayerApplyEffect player)
    {
       // handled in PlayerRelics
    }

    public override void Remove(PlayerApplyEffect player)
    {
       // removed automatically in TryUseRevive()
    }

    public override float GetStatChangeValue()
    {
        return reviveHealthPercent;
    }

    public ConsumableItemData thisRelicData;
}