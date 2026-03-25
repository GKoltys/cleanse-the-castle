using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RelicIconUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image icon;
    private string relicName;
    private string statDescription;
    private float stat;

    private BuffToolTipUI toolTip;

    public void Setup(Sprite icon, string relicName, string statDescription, float stat, BuffToolTipUI toolTip)
    {
        this.icon.sprite = icon;
        this.relicName = relicName;
        this.statDescription = statDescription;
        this.stat = stat;
        this.toolTip = toolTip;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (toolTip == null) return;

        toolTip.Show(statDescription, stat, gameObject.GetComponent<RectTransform>());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (toolTip == null) return;
        toolTip.Hide();
    }

    //public void SetStat(float stat) { this.stat = stat; }

    public string GetName => relicName;
}
