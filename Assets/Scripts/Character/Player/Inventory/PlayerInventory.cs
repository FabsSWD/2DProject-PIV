using UnityEngine;
public class PlayerInventory : MonoBehaviour
{
    public int coins = 0;
    private CoinSystem coinSystem;
    void Start()
    {
        coinSystem = FindObjectOfType<CoinSystem>();
        if(GameManager.Instance != null)
            coins = GameManager.Instance.coins;

        ApplyGameManagerValues();
    }

    public void ApplyGameManagerValues()
    {
        if (GameManager.Instance != null)
        {
            coins = GameManager.Instance.coins;
        }
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        if(GameManager.Instance != null)
            GameManager.Instance.coins = coins;
        if(coinSystem != null)
            coinSystem.CoinUpdate();
    }
    public bool SpendCoins(int amount)
    {
        if(coins >= amount)
        {
            coins -= amount;
            if(GameManager.Instance != null)
                GameManager.Instance.coins = coins;
            if(coinSystem != null)
                coinSystem.CoinUpdate();
            return true;
        }
        return false;
    }
}
