using TMPro;
using UnityEngine;

public class KeyCounterUI : MonoBehaviour
{
    [SerializeField] private TMP_Text keyCounterTMP;

    public void UpdateKeyCounter(int key)
    {
        if (keyCounterTMP == null)
        {
            Debug.LogError("floorCounterTMP is not assigned");
            return;
        }
        keyCounterTMP.SetText($"x {key}");
    }
}
