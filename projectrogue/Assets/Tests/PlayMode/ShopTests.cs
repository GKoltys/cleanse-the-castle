using NUnit.Framework;
using UnityEngine;

public class ShopTests
{
    // assert buying works and gold count is reduced
    [Test]
    public void TryBuyItem_PlayerHasEnoughGold_SpendsAppliesHudAndRemovesStock()
    {
        // reset static counter before test
        TestConsumableEffect.ApplyCalls = 0;

        var service = new ShopPurchaseService();

        var player = new FakeShopPlayer { Coins = 100 };
        var hud = new FakeShopHud();
        var stock = new FakeShopStock();

        // PlayerApplyEffect must be a real component
        var effectObj = new GameObject("PlayerEffect");
        var playerEffect = effectObj.AddComponent<PlayerApplyEffect>();

        var item = ScriptableObject.CreateInstance<ConsumableItemData>();
        item.itemName = "Potion";
        item.buyPrice = 10;
        item.effect = ScriptableObject.CreateInstance<TestConsumableEffect>();

        bool result = service.TryBuyItem(
            item,
            10,
            player,
            playerEffect,
            hud,
            stock
        );

        Assert.IsTrue(result);
        Assert.AreEqual(90, player.GetCoinCount);
        Assert.AreEqual(1, TestConsumableEffect.ApplyCalls);
        Assert.AreEqual(1, hud.BuffCalls);
        Assert.AreEqual(1, stock.RemoveCalls);
    }

    // assert buying does not work if low on gold and gold count is not reduced
    [Test]
    public void TryBuyItem_PlayerDoesNotHaveEnoughGold_DoesNothing()
    {
        // reset effect counter
        TestConsumableEffect.ApplyCalls = 0;

        var service = new ShopPurchaseService();

        var player = new FakeShopPlayer { Coins = 5 }; // not enough gold
        var hud = new FakeShopHud();
        var stock = new FakeShopStock();

        var effectObj = new GameObject("PlayerEffect");
        var playerEffect = effectObj.AddComponent<PlayerApplyEffect>();

        var item = ScriptableObject.CreateInstance<ConsumableItemData>();
        item.itemName = "Potion";
        item.buyPrice = 10;
        item.effect = ScriptableObject.CreateInstance<TestConsumableEffect>();

        bool result = service.TryBuyItem(
            item,
            10,
            player,
            playerEffect,
            hud,
            stock
        );

        // purchase should fail
        Assert.IsFalse(result);

        // gold unchanged
        Assert.AreEqual(5, player.GetCoinCount);

        // effect not applied
        Assert.AreEqual(0, TestConsumableEffect.ApplyCalls);

        // HUD not updated
        Assert.AreEqual(0, hud.BuffCalls);

        // stock unchanged
        Assert.AreEqual(0, stock.RemoveCalls);
    }
}

public class FakeShopPlayer : IShopPlayer
{
    public int Coins;

    public int GetCoinCount => Coins;

    public bool SpendGold(int amount)
    {
        if (Coins < amount)
            return false;

        Coins -= amount;
        return true;
    }
}


public class FakeShopHud : IShopHud
{
    public int BuffCalls;

    public void AddBuffIcon(ConsumableItemData item)
    {
        BuffCalls++;
    }
}


public class FakeShopStock : IShopStock
{
    public int RemoveCalls;

    public bool RemoveFromShopStock(ConsumableItemData item, int amount)
    {
        RemoveCalls++;
        return true;
    }
}

public class TestConsumableEffect : ConsumableEffect
{
    public static int ApplyCalls = 0;

    public override void Apply(PlayerApplyEffect player)
    {
        ApplyCalls++;
    }

    public override float GetStatChangeValue()
    {
        return 0f;
    }
}