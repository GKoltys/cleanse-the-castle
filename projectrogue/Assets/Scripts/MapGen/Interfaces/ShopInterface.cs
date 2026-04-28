public interface IShopPlayer
{
    int GetCoinCount { get; }
    bool SpendGold(int amount);
}

public interface IShopHud
{
    void AddBuffIcon(ConsumableItemData item);
}

public interface IShopStock
{
    bool RemoveFromShopStock(ConsumableItemData item, int amount);
}

public interface IShopEffect
{
    void Apply(PlayerApplyEffect playerEffect);
}

public class ShopPurchaseService
{
    public bool TryBuyItem(
        ConsumableItemData itemData,
        int price,
        IShopPlayer player,
        PlayerApplyEffect playerEffect,
        IShopHud hud,
        IShopStock stock)
    {
        if (itemData == null || player == null || stock == null)
            return false;

        if (!player.SpendGold(price))
            return false;

        if (itemData.effect != null && playerEffect != null)
            itemData.effect.Apply(playerEffect);

        if (hud != null)
            hud.AddBuffIcon(itemData);

        return stock.RemoveFromShopStock(itemData, 1);
    }
}