using UnityEngine;

[CreateAssetMenu(fileName = "Dodge", menuName = "Items/Effects/Dodge")]
public class SpeedDodgeRelicEffect : ConsumableEffect
{
    [SerializeField] private float dodgeChancePerSpeed;

    public override void Apply(PlayerApplyEffect player)
    {
        PlayerRelics relics = player.GetComponent<PlayerRelics>();
        if (relics != null)
        {
            relics.AddSpeedDodge(dodgeChancePerSpeed);
        }
    }

    public override void Remove(PlayerApplyEffect player)
    {
        PlayerRelics relics = player.GetComponent<PlayerRelics>();
        if (relics != null)
        {
            relics.RemoveSpeedDodge(dodgeChancePerSpeed);
        }
    }

    public override float GetStatChangeValue()
    {
        return dodgeChancePerSpeed;
    }
}