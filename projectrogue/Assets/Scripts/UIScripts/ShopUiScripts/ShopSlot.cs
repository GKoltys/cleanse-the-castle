using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    public ConsumableItemData currentItemData;
    public int itemPrice;
    public int itemQuantity;
    public bool isShopSlot = true;

    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text quantityText;

    // set shop slot ui elements
    private void Awake()
    {
        if (priceText == null)
        {
            Transform price = transform.Find("PriceText");
            if (price != null)
                priceText = price.GetComponent<TMP_Text>();
        }

        if (quantityText == null)
        {
            Transform quantity = transform.Find("QuantityText");
            if (quantity != null)
                quantityText = quantity.GetComponent<TMP_Text>();
        }

        if (itemNameText == null)
        {
            Transform nameText = transform.Find("ItemNameText");
            if (nameText != null)
                itemNameText = nameText.GetComponent<TMP_Text>();
        }

        if (itemIcon == null)
        {
            Transform icon = transform.Find("ItemIcon");
            if (icon != null)
                itemIcon = icon.GetComponent<Image>();
        }
    }

    // set shop slot items with ConsumableItemData assets
    public void UpdateDisplay()
    {
        if (currentItemData == null) return;

        if (priceText != null)
        {
            priceText.text = itemPrice.ToString();
        }

        if (quantityText != null)
        {
            quantityText.text = itemQuantity.ToString();
        }

        if (itemNameText != null)
        {
            itemNameText.text = currentItemData.itemName;
        }

        if (itemIcon != null)
        {
            itemIcon.sprite = currentItemData.icon;
        }
    }

    public void SetItem(ConsumableItemData itemData, int price, int quantity)
    {
        currentItemData = itemData;
        itemPrice = price;
        itemQuantity = quantity;
        UpdateDisplay();
    }

    public void OnBuyPressed()
    {
        if (!isShopSlot) return;
        if (currentItemData == null) return;
        if (ShopController.instance == null) return;

        Debug.Log("pressed");

        ShopController.instance.BuyItem(currentItemData, itemPrice);
    }
}