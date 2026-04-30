using System.Reflection;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITests
{

    // asserts that the coin counter displays the collected coin amount
    [Test]
    public void CoinCounterUI_UpdateCoinCounter_DisplaysCoinAmount()
    {
        GameObject obj = new GameObject("Coin Counter UI");

        CoinCounterUI coinCounterUI = obj.AddComponent<CoinCounterUI>();
        TextMeshProUGUI text = CreateUIText(obj);

        SetPrivateField(coinCounterUI, "coinCounterTMP", text);

        coinCounterUI.UpdateCoinCounter(5);

        Assert.AreEqual("x 5", text.text);
    }

    // asserts that the key counter displays the collected key amount
    [Test]
    public void KeyCounterUI_UpdateKeyCounter_DisplaysKeyAmount()
    {
        GameObject obj = new GameObject("Key Counter UI");

        KeyCounterUI keyCounterUI = obj.AddComponent<KeyCounterUI>();
        TextMeshProUGUI text = CreateUIText(obj);

        SetPrivateField(keyCounterUI, "keyCounterTMP", text);

        keyCounterUI.UpdateKeyCounter(3);

        Assert.AreEqual("x 3", text.text);
    }

    // asserts that the floor counter displays the current floor number
    [Test]
    public void FloorCounterUI_UpdateFloorCounter_DisplaysFloorNumber()
    {
        GameObject obj = new GameObject("Floor Counter UI");

        FloorCounterUI floorCounterUI = obj.AddComponent<FloorCounterUI>();
        TextMeshProUGUI text = CreateUIText(obj);

        SetPrivateField(floorCounterUI, "floorCounterTMP", text);

        floorCounterUI.UpdateFloorCounter(7);

        Assert.AreEqual("Floor: 7", text.text);
    }

    // asserts that the damage popup displays the damage amount dealt
    [Test]
    public void DamagePopUp_Setup_DisplaysDamageAmount()
    {
        GameObject obj = new GameObject("Damage Popup");

        DamagePopUp damagePopUp = obj.AddComponent<DamagePopUp>();
        TextMeshPro textMesh = CreateWorldText(obj);

        SetPrivateField(damagePopUp, "textMesh", textMesh);

        damagePopUp.Setup(25f);

        Assert.AreEqual("25", textMesh.text);
    }

    // asserts that the damage popup displays decimal damage amounts correctly
    [Test]
    public void DamagePopUp_Setup_DisplaysDecimalDamageAmount()
    {
        GameObject obj = new GameObject("Damage Popup");

        DamagePopUp damagePopUp = obj.AddComponent<DamagePopUp>();
        TextMeshPro textMesh = CreateWorldText(obj);

        SetPrivateField(damagePopUp, "textMesh", textMesh);

        damagePopUp.Setup(12.5f);

        Assert.AreEqual("12.5", textMesh.text);
    }

    // asserts that the health bar width is reduced based on current health
    [Test]
    public void HealthBarUI_SetHealth_UpdatesHealthBarWidth()
    {
        GameObject obj = new GameObject("Health Bar UI");

        RectTransform rectTransform = CreateRectTransform(obj, 200f, 20f);
        HealthBarUI healthBarUI = obj.AddComponent<HealthBarUI>();

        InvokePrivateMethod(healthBarUI, "Awake");

        healthBarUI.SetMaxHealth(100f);
        healthBarUI.SetHealth(50f);

        Assert.AreEqual(100f, rectTransform.sizeDelta.x);
    }

    // asserts that the health bar width becomes zero when health is zero
    [Test]
    public void HealthBarUI_SetHealth_ZeroHealthSetsWidthToZero()
    {
        GameObject obj = new GameObject("Health Bar UI");

        RectTransform rectTransform = CreateRectTransform(obj, 200f, 20f);
        HealthBarUI healthBarUI = obj.AddComponent<HealthBarUI>();

        InvokePrivateMethod(healthBarUI, "Awake");

        healthBarUI.SetMaxHealth(100f);
        healthBarUI.SetHealth(0f);

        Assert.AreEqual(0f, rectTransform.sizeDelta.x);
    }

    // asserts that BuffToolTipUI displays buff stat text and becomes active
    [Test]
    public void BuffToolTipUI_Show_DisplaysStatDescriptionAndActivatesTooltip()
    {
        GameObject tooltipObj = CreateToolTip(
            out TextMeshProUGUI descriptionText,
            out TextMeshProUGUI itemNameText,
            out RectTransform tooltipRect,
            out RectTransform containerRect
        );

        BuffToolTipUI tooltip = tooltipObj.AddComponent<BuffToolTipUI>();

        SetPrivateField(tooltip, "descriptionText", descriptionText);
        SetPrivateField(tooltip, "itemNameText", itemNameText);
        SetPrivateField(tooltip, "rectTransform", tooltipRect);
        SetPrivateField(tooltip, "buffContainerRectTransform", containerRect);

        GameObject iconObj = new GameObject("Buff Icon");
        RectTransform iconRect = CreateRectTransform(iconObj, 50f, 50f);

        tooltipObj.SetActive(false);

        tooltip.Show("Damage +", 2f, iconRect);

        Assert.AreEqual("Damage +2", descriptionText.text);
        Assert.IsTrue(tooltipObj.activeSelf);
    }

    // asserts that BuffToolTipUI displays relic name and description
    [Test]
    public void BuffToolTipUI_ShowRelic_DisplaysRelicNameAndDescription()
    {
        GameObject tooltipObj = CreateToolTip(
            out TextMeshProUGUI descriptionText,
            out TextMeshProUGUI itemNameText,
            out RectTransform tooltipRect,
            out RectTransform containerRect
        );

        BuffToolTipUI tooltip = tooltipObj.AddComponent<BuffToolTipUI>();

        SetPrivateField(tooltip, "descriptionText", descriptionText);
        SetPrivateField(tooltip, "itemNameText", itemNameText);
        SetPrivateField(tooltip, "rectTransform", tooltipRect);
        SetPrivateField(tooltip, "buffContainerRectTransform", containerRect);

        GameObject iconObj = new GameObject("Relic Icon");
        RectTransform iconRect = CreateRectTransform(iconObj, 50f, 50f);

        tooltipObj.SetActive(false);

        tooltip.ShowRelic("Ruby Ring", "Increases damage", iconRect);

        Assert.AreEqual("Ruby Ring", itemNameText.text);
        Assert.AreEqual("Increases damage", descriptionText.text);
        Assert.IsTrue(tooltipObj.activeSelf);
    }

    // asserts that BuffToolTipUI hides the tooltip object
    [Test]
    public void BuffToolTipUI_Hide_DeactivatesTooltip()
    {
        GameObject tooltipObj = CreateToolTip(
            out _,
            out _,
            out _,
            out _
        );

        BuffToolTipUI tooltip = tooltipObj.AddComponent<BuffToolTipUI>();

        tooltipObj.SetActive(true);

        tooltip.Hide();

        Assert.IsFalse(tooltipObj.activeSelf);
    }

    // asserts that BuffIconUI stores and exposes the buff name after setup
    [Test]
    public void BuffIconUI_Setup_StoresBuffName()
    {
        GameObject obj = new GameObject("Buff Icon");

        BuffIconUI buffIconUI = obj.AddComponent<BuffIconUI>();
        Image image = obj.AddComponent<Image>();
        Sprite sprite = CreateTestSprite();

        SetPrivateField(buffIconUI, "icon", image);

        buffIconUI.Setup(sprite, "Strength Buff", "Damage +", 2f, null, false);

        Assert.AreEqual("Strength Buff", buffIconUI.GetName);
        Assert.AreEqual(sprite, image.sprite);
    }

    // asserts that BuffIconUI updates the stored stat value without throwing errors
    [Test]
    public void BuffIconUI_SetStat_DoesNotThrow()
    {
        GameObject obj = new GameObject("Buff Icon");

        BuffIconUI buffIconUI = obj.AddComponent<BuffIconUI>();

        Assert.DoesNotThrow(() =>
        {
            buffIconUI.SetStat(5f);
        });
    }

    // asserts that RelicIconUI stores and exposes the relic name after setup
    [Test]
    public void RelicIconUI_Setup_StoresRelicName()
    {
        GameObject obj = new GameObject("Relic Icon");

        RelicIconUI relicIconUI = obj.AddComponent<RelicIconUI>();
        Image image = obj.AddComponent<Image>();
        Sprite sprite = CreateTestSprite();

        SetPrivateField(relicIconUI, "icon", image);

        relicIconUI.Setup(sprite, "Golden Feather", "Dodge chance +", 10f, null);

        Assert.AreEqual("Golden Feather", relicIconUI.GetName);
        Assert.AreEqual(sprite, image.sprite);
    }

    // helper functions
    private TextMeshProUGUI CreateUIText(GameObject obj)
    {
        return obj.AddComponent<TextMeshProUGUI>();
    }

    private TextMeshPro CreateWorldText(GameObject obj)
    {
        return obj.AddComponent<TextMeshPro>();
    }

    private RectTransform CreateRectTransform(GameObject obj, float width, float height)
    {
        RectTransform rectTransform = obj.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(width, height);

        return rectTransform;
    }

    private Sprite CreateTestSprite()
    {
        return Sprite.Create(
            Texture2D.whiteTexture,
            new Rect(0, 0, 1, 1),
            Vector2.zero
        );
    }

    private GameObject CreateToolTip(
        out TextMeshProUGUI descriptionText,
        out TextMeshProUGUI itemNameText,
        out RectTransform tooltipRect,
        out RectTransform containerRect)
    {
        GameObject tooltipObj = new GameObject("Tooltip");
        tooltipRect = CreateRectTransform(tooltipObj, 200f, 100f);

        GameObject descriptionObj = new GameObject("Description Text");
        descriptionText = CreateUIText(descriptionObj);

        GameObject itemNameObj = new GameObject("Item Name Text");
        itemNameText = CreateUIText(itemNameObj);

        GameObject containerObj = new GameObject("Buff Container");
        containerRect = CreateRectTransform(containerObj, 500f, 100f);

        return tooltipObj;
    }

    private void SetPrivateField(object target, string fieldName, object value)
    {
        FieldInfo field = target.GetType().GetField(
            fieldName,
            BindingFlags.NonPublic | BindingFlags.Instance
        );

        field.SetValue(target, value);
    }

    private void InvokePrivateMethod(object target, string methodName)
    {
        MethodInfo method = target.GetType().GetMethod(
            methodName,
            BindingFlags.NonPublic | BindingFlags.Instance
        );

        method.Invoke(target, null);
    }
}