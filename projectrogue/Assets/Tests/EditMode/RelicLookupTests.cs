using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

public class RelicLookupTests
{
    // assert relic consumable data returns given relic id
    [Test]
    public void GetRelicByName_ReturnsCorrectRelic_WhenNameExists()
    {
        var lookup = ScriptableObject.CreateInstance<RelicLookup>();

        var relic = ScriptableObject.CreateInstance<ConsumableItemData>();
        relic.itemName = "fire_relic";

        SetRelicList(lookup, relic);

        var result = lookup.GetRelicByName("fire_relic");

        Assert.AreEqual(relic, result);
    }

    // assert null result if relic id does not exist in lookup
    [Test]
    public void GetRelicByName_ReturnsNull_WhenRelicDoesNotExist()
    {
        var lookup = ScriptableObject.CreateInstance<RelicLookup>();

        var relic = ScriptableObject.CreateInstance<ConsumableItemData>();
        relic.itemName = "ice_relic";

        SetRelicList(lookup, relic);

        var result = lookup.GetRelicByName("fire_relic");

        Assert.IsNull(result);
    }

    // assert relic consumable data returns given relic id
    [Test]
    public void GetRelicByName_IgnoresNullEntries()
    {
        var lookup = ScriptableObject.CreateInstance<RelicLookup>();

        var relic = ScriptableObject.CreateInstance<ConsumableItemData>();
        relic.itemName = "fire_relic";

        SetRelicList(lookup, null, relic);

        var result = lookup.GetRelicByName("fire_relic");

        Assert.AreEqual(relic, result);
    }

    // assert relic data is saved to savedata if exists in relic lookup
    [Test]
    public void SaveData_RelicIdCanBeResolvedByLookup()
    {
        var lookup = ScriptableObject.CreateInstance<RelicLookup>();

        var relic = ScriptableObject.CreateInstance<ConsumableItemData>();
        relic.itemName = "fire_relic";

        SetRelicList(lookup, relic);

        var saveData = new SaveData();
        saveData.playerRelicIds.Add("fire_relic");

        var result = lookup.GetRelicByName(saveData.playerRelicIds[0]);

        Assert.AreEqual(relic, result);
    }

    // assert relic data is not saved to savedata if not exists in relic lookup
    [Test]
    public void SaveData_LookupReturnsNullIfInvalidRelicID()
    {
        var lookup = ScriptableObject.CreateInstance<RelicLookup>();

        var relic = ScriptableObject.CreateInstance<ConsumableItemData>();
        relic.itemName = "fire_relic";

        SetRelicList(lookup, relic);

        var saveData = new SaveData();
        saveData.playerRelicIds.Add("ice_relic");

        var result = lookup.GetRelicByName(saveData.playerRelicIds[0]);

        Assert.IsNull(result);
    }

    private void SetRelicList(RelicLookup lookup, params ConsumableItemData[] relics)
    {
        // https://learn.microsoft.com/en-us/dotnet/api/system.reflection.bindingflags?view=net-10.0
        typeof(RelicLookup)
            .GetField("relics", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(lookup, new List<ConsumableItemData>(relics));
    }
}