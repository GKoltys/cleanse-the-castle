using TMPro;
using UnityEngine;

public class CoinCounterUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinCounterTMP;
    
    public void UpdateCoinCounter(int coins)
    {
        if (coinCounterTMP == null)
        {
            Debug.LogError("floorCounterTMP is not assigned");
            return;
        }
        coinCounterTMP.SetText($"x {coins}");
    }
}
