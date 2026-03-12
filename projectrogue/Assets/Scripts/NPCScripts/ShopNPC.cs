using System.Collections.Generic;
using UnityEngine;

public class ShopNPC : MonoBehaviour, IInteractable
{
    // shop npc references
    public GameObject interactionIcon;
    public string shopID = "shop_merchant_01";
    public string shopkeeperName = "Merchant";

    // set a list of items to sell on default and current when something is sold
    public List<ShopStockItem> defaultShopStock = new();
    private List<ShopStockItem> currentShopStock = new();

    private bool isInitialized = false;

    private void Awake()
    {
        if (interactionIcon != null)
        {
            interactionIcon.SetActive(false);
        }

        InitializeShop();
    }

    // fill the shop ui grid with default shop stock items
    private void InitializeShop()
    {
        if (isInitialized) return;

        currentShopStock = new List<ShopStockItem>();

        // copy default stock
        List<ShopStockItem> shuffled = new List<ShopStockItem>(defaultShopStock);

        // shuffle
        for (int i = 0; i < shuffled.Count; i++)
        {
            int swapIndex = Random.Range(i, shuffled.Count);
            (shuffled[i], shuffled[swapIndex]) = (shuffled[swapIndex], shuffled[i]);
        }
        
        // could change to be a random number of items in shop?
        foreach (var item in shuffled)
        {
            if (item == null || item.itemData == null) continue;

            currentShopStock.Add(new ShopStockItem
            {
                itemData = item.itemData,
                quantity = item.quantity
            });
        }
        isInitialized = true;
    }

    public bool CanInteract()
    {
        return true;

    }

    public void ShowCanInteract(bool show)
    {
        interactionIcon.SetActive(show);
    }

    // if shop ui is open then close, if not open then open
    public void Interact(GameObject last)
    {
        if (ShopController.instance == null) return;

        if (ShopController.instance.shopPanel.activeSelf)
        {
            ShopController.instance.CloseShop();
        }
        else
        {
            ShopController.instance.OpenShop(this);
        }
    }

    public List<ShopStockItem> GetCurrentStock()
    {
        InitializeShop();
        return currentShopStock;
    }

    public void SetStock(List<ShopStockItem> stock)
    {
        currentShopStock = stock;
    }

    public void AddToStock(ConsumableItemData itemData, int quantity)
    {
        if (itemData == null || quantity <= 0) return;

        ShopStockItem existing = currentShopStock.Find(s => s.itemData == itemData);

        if (existing != null)
        {
            existing.quantity += quantity;
        }
        else
        {
            currentShopStock.Add(new ShopStockItem
            {
                itemData = itemData,
                quantity = quantity
            });
        }
    }

    public bool RemoveFromShopStock(ConsumableItemData itemData, int quantity)
    {
        InitializeShop();

        if (itemData == null || quantity <= 0) return false;

        ShopStockItem existing = currentShopStock.Find(s => s.itemData == itemData);

        if (existing != null && existing.quantity >= quantity)
        {
            existing.quantity -= quantity;

            if (existing.quantity <= 0)
            {
                currentShopStock.Remove(existing);
            }

            return true;
        }

        return false;
    }
}