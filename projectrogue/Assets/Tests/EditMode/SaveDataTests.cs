using NUnit.Framework;
using UnityEngine;

public class SaveDataTests
{
    // assert save data saves as json file
    [Test]
    public void SaveData_SerializesAndDeserializesCorrectly()
    {
        SaveData save = new SaveData
        {
            playerPosistion = new Vector3(1, 2, 3),
            playerFloorCount = 5,
            playerSpeed = 6f,
            playerIFrameSeconds = 0.75f,
            playerMaxHealth = 120f,
            playerHealth = 80f,
            playerCoinCount = 12,
            playerKeyCount = 2,
            playerWeaponId = 3,
            playerDamageMultiplier = 1.5f
        };

        save.playerRelicIds.Add("venom_orb");

        string json = JsonUtility.ToJson(save);
        SaveData loaded = JsonUtility.FromJson<SaveData>(json);

        Assert.AreEqual(new Vector3(1, 2, 3), loaded.playerPosistion);
        Assert.AreEqual(5, loaded.playerFloorCount);
        Assert.AreEqual(6f, loaded.playerSpeed);
        Assert.AreEqual(0.75f, loaded.playerIFrameSeconds);
        Assert.AreEqual(120f, loaded.playerMaxHealth);
        Assert.AreEqual(80f, loaded.playerHealth);
        Assert.AreEqual(12, loaded.playerCoinCount);
        Assert.AreEqual(2, loaded.playerKeyCount);
        Assert.AreEqual(3, loaded.playerWeaponId);
        Assert.AreEqual(1.5f, loaded.playerDamageMultiplier);
        Assert.AreEqual("venom_orb", loaded.playerRelicIds[0]);
    }
}