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
        interactionIcon.SetActive(false);
    }

    void Start()
    {
        InitializeShop();
    }
    
    // fill the shop ui grid with default shop stock items
    private void InitializeShop()
    {
        if (isInitialized) return;

        currentShopStock = new List<ShopStockItem>();
        foreach (var item in defaultShopStock)
        {
            currentShopStock.Add(new ShopStockItem
            {
                itemID = item.itemID,
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
        return currentShopStock;
    }

    public void SetStock(List<ShopStockItem> stock)
    {
        currentShopStock = stock;
    }

    public void AddToStock(int itemID, int quantity)
    {
        ShopStockItem existing = currentShopStock.Find(s => s.itemID == itemID);
        if (existing != null) {
            existing.quantity += quantity;
        }
        else
        {
            currentShopStock.Add(new ShopStockItem { itemID = itemID, quantity = quantity });
        }
    }

    public bool RemoveFromShopStock(int itemID, int quantity)
    {
        ShopStockItem existing = currentShopStock.Find(s => s.itemID == itemID);
        if (existing != null && existing.quantity >= quantity) {
            existing.quantity -= quantity;
            return true;
        }
        else
        {
            return false;
        }
    }
}