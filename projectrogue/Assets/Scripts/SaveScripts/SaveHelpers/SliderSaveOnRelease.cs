using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class SliderSaveOnRelease : MonoBehaviour, IPointerUpHandler, IEndDragHandler
{
    public void OnPointerUp(PointerEventData eventData)
    {
        Save();
    }

    // Controller support
    public void OnEndDrag(PointerEventData eventData)
    {
        Save();
    }

    private void Save()
    {
        if (SaveSettings.Instance != null) SaveSettings.Instance.Save();
    }
}
