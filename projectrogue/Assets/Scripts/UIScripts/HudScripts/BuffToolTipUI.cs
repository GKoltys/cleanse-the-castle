using TMPro;
using UnityEngine;

public class BuffToolTipUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectTransform buffContainerRectTransform;

    public void Show(string statDescription, float stat, RectTransform buffIconTransform)
    {
        descriptionText.text = statDescription + stat;

        Vector3[] corners = new Vector3[4];
        buffContainerRectTransform.GetWorldCorners(corners);

        float containerBottomY = corners[0].y;
        float iconCentreX = buffIconTransform.position.x;

        float tooltipHeight = buffIconTransform.rect.height;
        rectTransform.position = new Vector2(iconCentreX, containerBottomY - tooltipHeight);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
