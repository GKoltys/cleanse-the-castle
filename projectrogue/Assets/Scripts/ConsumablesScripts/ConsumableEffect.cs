using UnityEngine;

public abstract class ConsumableEffect : ScriptableObject
{
    public abstract void Apply(PlayerBase player);
}
