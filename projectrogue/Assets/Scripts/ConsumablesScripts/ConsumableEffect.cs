using UnityEngine;

public abstract class ConsumableEffect : ScriptableObject
{
    public abstract void Apply(PlayerApplyEffect player);
    public virtual void Remove(PlayerApplyEffect player)
    {
        // option override used for relic type consumables
    }
    public abstract float GetStatChangeValue();
}
