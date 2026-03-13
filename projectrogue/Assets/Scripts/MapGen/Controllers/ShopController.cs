using TMPro;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField] private PlayerBase playerBase;
    [SerializeField] private PlayerApplyEffect playerEffect; // You will need to use this to apply any effect onto the player instead of PlayerBase
    public static ShopController instance;

    // shop ui
    public GameObject shopPanel;
    public Transform shopGrid;
    public GameObject shopSlotPrefab;
    public TMP_Text playerMoneyText, shopTitleText, shopStatusText, itemDescriptionText;

    // current instance of shop, can have multiple different shops
    private ShopNPC currentShop;

    private void Awake()
    {
        instance = this;
    }

    // set shop ui to hide on start and get reference to player gold
    void Start()
    {
        shopPanel.SetActive(false);

        if (playerBase != null) {
        
            UpdateMoneyDisplay(playerBase.GetCoinCount);
        }
        
    }

    // update the player's current money in the shop ui
    private void UpdateMoneyDisplay(int amount)
    {
        if (playerMoneyText != null)
        {
            playerMoneyText.text = amount.ToString();
        }
    }

    // show the shop ui of a given shop npc
    public void OpenShop(ShopNPC shop)
    {
        currentShop = shop;
        shopPanel.SetActive(true);
        if (shopTitleText != null)
        {
            shopTitleText.text = shop.shopkeeperName + "'s Shop";
        }
        if (playerBase != null)
        {
            UpdateMoneyDisplay(playerBase.GetCoinCount);
        }
        RefreshShopDisplay();
    }

    // close the shop ui of a given shop npc
    public void CloseShop()
    {
        shopPanel.SetActive(false);
        currentShop = null;
    }

    // clear the shop grid and re add shop slots with current stock
    public void RefreshShopDisplay()
    {
        if (currentShop == null)
        {
            return;
        }

        foreach (Transform child in shopGrid)
        {
            Destroy(child.gameObject);
        }

        foreach (var stockItem in currentShop.GetCurrentStock())
        {
            if (stockItem.quantity <= 0 || stockItem.itemData == null) continue;

            CreateShopSlot(shopGrid, stockItem.itemData, stockItem.quantity, true);
        }
    }

    // create a single shop slot ui element for an item
    private void CreateShopSlot(Transform grid, ConsumableItemData itemData, int quantity, bool isShop, ShopSlot originalSlot = null)
    {
        if (itemData == null) { return; }

        // create a new shop slot prefab
        GameObject slotObj = Instantiate(shopSlotPrefab, grid);
        ShopSlot slot = slotObj.GetComponent<ShopSlot>();

        slot.isShopSlot = isShop;
        slot.SetItem(itemData, itemData.buyPrice, quantity);
    }

    // buys the selected item
    public void BuyItem(ConsumableItemData itemData, int price)
    {

        if (currentShop == null || itemData == null || playerBase == null)
        {
            Debug.Log("Null error");
            return;
        }

        // not enough gold
        if (!playerBase.SpendGold(price))
        {
            ShowStatus("Not enough gold!");
            return;
        }

        // apply effect of item to player
        itemData.effect.Apply(playerEffect);

        // remove one of the purchased item from the shop stock
        bool removed = currentShop.RemoveFromShopStock(itemData, 1);

        Debug.Log($"Bought {itemData.itemName} for {price} gold.");

        // update player money count
        UpdateMoneyDisplay(playerBase.GetCoinCount);
        // refresh shop grid
        RefreshShopDisplay();
        ShowStatus("Item bought!");
    }

    // display a text in the shop ui on successful/unsuccessful shop purchase
    private void ShowStatus(string message)
    {
        if (shopStatusText == null) return;

        shopStatusText.text = message;
        StopAllCoroutines();
        StartCoroutine(ClearStatusAfterDelay());
    }

    private System.Collections.IEnumerator ClearStatusAfterDelay()
    {
        yield return new WaitForSeconds(2f);

        if (shopStatusText != null)
        {
            shopStatusText.text = "";
        }
    }

    public void ShowItemDescription(string description)
    {
        if (itemDescriptionText != null)
        {
            itemDescriptionText.text = description;
        }
    }

    public void ClearItemDescription()
    {
        if (itemDescriptionText != null)
        {
            itemDescriptionText.text = "";
        }
    }
}
    