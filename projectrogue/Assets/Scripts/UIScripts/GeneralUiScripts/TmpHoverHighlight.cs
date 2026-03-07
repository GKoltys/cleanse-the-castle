using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TmpHoverHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Color hoverColour = Color.red;
    private bool active = true;
    
    private TMP_Text tmp;
    private Color defaultColour;

    private void Awake()
    {
        tmp = GetComponent<TMP_Text>();
        defaultColour = tmp.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!active) return;
        tmp.color = hoverColour;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!active) return;
        tmp.color = defaultColour;
    }

    public void SetActive(bool active)
    {
        this.active = active;
    }
}
