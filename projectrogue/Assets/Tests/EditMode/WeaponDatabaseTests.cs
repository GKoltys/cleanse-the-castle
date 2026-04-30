using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Reflection;

public class WeaponDatabaseTests
{
    // asserts correct weapondata returns from id in weapondatabase
    [Test]
    public void GetWeaponById_ReturnsCorrectWeapon_WhenIdExists()
    {
        var database = ScriptableObject.CreateInstance<WeaponDatabase>();

        var sword = CreateWeapon(id: 1, name: "Sword");
        var axe = CreateWeapon(id: 2, name: "Axe");

        SetWeapons(database, sword, axe);

        WeaponData result = database.GetWeaponById(2);

        Assert.AreEqual(axe, result);
        Assert.AreEqual("Axe", result.weaponName);
    }

    // asserts returns null if weapondata id does not exist in weapondatabase
    [Test]
    public void GetWeaponById_ReturnsNull_WhenIdDoesNotExist()
    {
        var database = ScriptableObject.CreateInstance<WeaponDatabase>();

        var sword = CreateWeapon(id: 1, name: "Sword");
        SetWeapons(database, sword);

        LogAssert.Expect(LogType.Warning, "Weapon with ID 99 not found");

        WeaponData result = database.GetWeaponById(99);

        Assert.IsNull(result);
    }

    // helper functions
    private static WeaponData CreateWeapon(int id, string name)
    {
        var weapon = ScriptableObject.CreateInstance<WeaponData>();
        weapon.id = id;
        weapon.weaponName = name;
        return weapon;
    }

    private static void SetWeapons(WeaponDatabase database, params WeaponData[] weapons)
    {
        typeof(WeaponDatabase)
            .GetField("weapons", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(database, weapons);
    }
}