using NUnit.Framework;
using UnityEngine;
using System.IO;

public class SaveTests
{
    // assert when save files is wiped it sets values to default
    [Test]
    public void WipeSaveFile_CreatesDefaultJsonSave()
    {
        var controller = CreateSaveController();

        string path = GetTestSavePath("wipeSaveTest.json");
        controller.SetSaveLocation(path);

        controller.WipeSaveFile();

        Assert.IsTrue(File.Exists(path));

        SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(path));

        Assert.AreEqual(Vector3.zero, data.playerPosistion);
        Assert.AreEqual(0, data.playerFloorCount);
        Assert.AreEqual(5f, data.playerSpeed);
        Assert.AreEqual(0.5f, data.playerIFrameSeconds);
        Assert.AreEqual(100f, data.playerMaxHealth);
        Assert.AreEqual(100f, data.playerHealth);
        Assert.AreEqual(0, data.playerCoinCount);
        Assert.AreEqual(0, data.playerKeyCount);
        Assert.AreEqual(0, data.playerWeaponId);
        Assert.AreEqual(1f, data.playerDamageMultiplier);

        DeleteFileIfExists(path);
    }

    // assert floor count is saved properly
    [Test]
    public void GetLastFloor_WhenSaveExists_ReturnsSavedFloor()
    {
        var controller = CreateSaveController();

        string path = GetTestSavePath("lastFloorTest.json");
        controller.SetSaveLocation(path);

        SaveData data = new SaveData
        {
            playerFloorCount = 7
        };

        File.WriteAllText(path, JsonUtility.ToJson(data));

        int result = controller.GetLastFloor();

        Assert.AreEqual(7, result);

        DeleteFileIfExists(path);
    }

    // assert floor count is zero if no save file exists
    [Test]
    public void GetLastFloor_WhenNoSaveExists_ReturnsZero()
    {
        var controller = CreateSaveController();

        string path = GetTestSavePath("missingSaveTest.json");
        DeleteFileIfExists(path);

        controller.SetSaveLocation(path);

        int result = controller.GetLastFloor();

        Assert.AreEqual(0, result);
    }



    // helper functions
    private static SaveController CreateSaveController()
    {
        var obj = new GameObject("SaveController");
        return obj.AddComponent<SaveController>();
    }

    private static string GetTestSavePath(string fileName)
    {
        return Path.Combine(Application.temporaryCachePath, fileName);
    }

    private static void DeleteFileIfExists(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}