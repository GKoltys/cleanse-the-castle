using TMPro;
using UnityEngine;

public class FloorCounterUI : MonoBehaviour
{
    [SerializeField] private TMP_Text floorCounterTMP;

    public void UpdateFloorCounter(int floor)
    {
        if (floorCounterTMP == null)
        {
            Debug.LogError("floorCounterTMP is not assigned");
            return;
        }
        floorCounterTMP.SetText($"Floor: {floor}");
    }
}
