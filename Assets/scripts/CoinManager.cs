
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    [Header("Coin Settings")]
    public int totalCoins = 0;

    [Header("UI")]
    public Text coinText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateCoinUI();
    }

    public void AddCoins(int amount)
    {
        totalCoins += amount;

        UpdateCoinUI();

        Debug.Log("Total Coins: " + totalCoins);
    }

    void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = " " + totalCoins;
        }
    }
}
