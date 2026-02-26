using TMPro;
using UnityEngine;

public class CoinCounterUI : MonoBehaviour
{
    private int Counter;
    [SerializeField] private TMP_Text coinCounterTMP;
    
    public void UpdateCoinCounter(int coins)
    {
        if (coinCounterTMP == null)
        {
            Debug.LogError("coinCounterTMP is not assigned");
            return;
        }

        Counter = coins;
        coinCounterTMP.SetText($"x {Counter}");
    }
}
