using UnityEngine;

public abstract class ConsumableEffect : ScriptableObject
{
    public abstract void Apply(PlayerApplyEffect player);
    public abstract void Remove(PlayerApplyEffect player);
    public abstract float GetStatChangeValue();
}
