using TMPro;
using UnityEngine;

public class BuffToolTipUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI itemNameText;
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

    public void ShowRelic (string relicName, string relicDescription, RectTransform buffIconTransform)
    {
        itemNameText.text = relicName;
        descriptionText.text = relicDescription;

        Vector3[] corners = new Vector3[4];
        buffIconTransform.GetWorldCorners(corners);

        float iconRightX = corners[2].x;
        float iconCenterY = (corners[0].y + corners[1].y) * 0.5f;

        float padding = 100f;
        float tooltipHalfWidth = rectTransform.rect.width * rectTransform.lossyScale.x * 0.5f;

        rectTransform.position = new Vector3(
            iconRightX + padding + tooltipHalfWidth,
            iconCenterY,
            0f
        );

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
