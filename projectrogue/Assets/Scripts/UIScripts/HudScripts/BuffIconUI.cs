using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuffIconUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image icon;
    private string buffName;
    private string statDescription;
    private float stat;
    private bool isRelic;

    private BuffToolTipUI toolTip;

    public void Setup(Sprite icon, string buffName, string statDescription, float stat, BuffToolTipUI toolTip, bool isRelic)
    {
        this.icon.sprite = icon;
        this.buffName = buffName;
        this.statDescription = statDescription;
        this.stat = stat;
        this.toolTip = toolTip;
        this.isRelic = isRelic;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (toolTip == null) return;

        if (isRelic)
        {
            toolTip.ShowRelic(buffName, statDescription, GetComponent<RectTransform>());
        } else {
            toolTip.Show(statDescription, stat, gameObject.GetComponent<RectTransform>());
        }
  
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (toolTip == null) return;
        toolTip.Hide();
    }

    public void SetStat(float stat) { this.stat = stat; }

    public string GetName => buffName;
}
