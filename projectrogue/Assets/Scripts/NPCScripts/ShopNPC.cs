using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopNPC : MonoBehaviour, IInteractable, IShopStock
{
    private PlayerInput playerInput;

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

    private void Start()
    {
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
    }

    // fill the shop ui grid with default shop stock items
    private void InitializeShop()
    {
        if (isInitialized) return;

        currentShopStock = new List<ShopStockItem>();

        // copy default stock
        List<ShopStockItem> shuffled = new List<ShopStockItem>(defaultShopStock);

        // shuffle
        // could possibly sort by price or something?
        for (int i = 0; i < shuffled.Count; i++)
        {
            int swapIndex = Random.Range(i, shuffled.Count);
            (shuffled[i], shuffled[swapIndex]) = (shuffled[swapIndex], shuffled[i]);
        }

        // add a range of 1-3 items to the shop
        int randomCount = Random.Range(1, 4);

        for (int i = 0; i < randomCount; i++)
        {
            var item = shuffled[i];

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
            Debug.Log("Shop closed, inputs back on");
            ShopController.instance.CloseShop();
            playerInput.actions["Attack"].Enable();
            playerInput.actions["Move"].Enable();
        }
        else
        {
            Debug.Log("Shop opened, inputs off");
            ShopController.instance.OpenShop(this);
            playerInput.actions["Attack"].Disable();
            playerInput.actions["Move"].Disable();
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