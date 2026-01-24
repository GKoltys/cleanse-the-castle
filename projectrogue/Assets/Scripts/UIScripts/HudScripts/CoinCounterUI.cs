using TMPro;
using UnityEngine;

public class CoinCounterUI : MonoBehaviour
{
    private int Counter;
    [SerializeField] private TMP_Text coinCounterTMP;
    
    public void SetCoins(int coins)
    {
        if (coinCounterTMP == null)
        {
            Debug.LogError("coinCounterTMP is not assigned");
            return;
        }

        Counter = coins;
        coinCounterTMP.SetText($"x {Counter}");
    }

    public void UpdateCounter(int amount)
    {
        Counter += amount;
        coinCounterTMP.SetText($"x {Counter}");
    }
}
