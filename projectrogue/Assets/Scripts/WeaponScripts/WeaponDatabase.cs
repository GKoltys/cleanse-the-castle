using UnityEngine;

// https://www.youtube.com/watch?v=iLrF_tnB__A

[CreateAssetMenu(menuName = "Weapons/Weapon Database")]
public class WeaponDatabase : ScriptableObject
{
    [SerializeField] private WeaponData[] weapons;

    public WeaponData GetWeaponById(int id)
    {
        foreach (var weapon in weapons)
        {
            if (weapon.id == id) return weapon;
        }

        Debug.LogWarning($"Weapon with ID {id} not found");
        return null;
    }
}
