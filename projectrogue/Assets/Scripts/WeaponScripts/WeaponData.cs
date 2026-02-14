using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public float damage = 15f;
    public float cooldown = 0.5f;
    public float range = 1f;
    public float knockbackForce = 4f;
    public Vector2 hitBoxSize = new(1f, 0.6f);
    public string attackTrigger = "Attack";
}
