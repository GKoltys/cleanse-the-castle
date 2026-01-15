using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TmpHoverHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Color hoverColour = Color.red;
    
    private TMP_Text tmp;
    private Color defaultColour;

    private void Awake()
    {
        tmp = GetComponent<TMP_Text>();
        defaultColour = tmp.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tmp.color = hoverColour;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tmp.color = defaultColour;
    }
}
