using TMPro;
using UnityEngine;

public class ShopController : MonoBehaviour
{

    [SerializeField] private PlayerStats playerStats;
    public static ShopController instance;

    // shop ui
    public GameObject shopPanel;
    public Transform shopGrid;
    public GameObject shopSlotPrefab;
    public TMP_Text playerMoneyText, shopTitleText;

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
  
        if (playerStats != null) {
        
            UpdateMoneyDisplay(playerStats.GetCoinCount);
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
    }

    // close the shop ui of a given shop npc
    public void CloseShop()
    {
        shopPanel.SetActive(false);
        currentShop = null;
    }
}
