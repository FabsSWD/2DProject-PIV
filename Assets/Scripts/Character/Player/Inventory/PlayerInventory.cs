using UnityEngine;
public class PlayerInventory : MonoBehaviour
{
    public int coins = 0;
    private CoinSystem coinSystem;
    void Start()
    {
        coinSystem = FindObjectOfType<CoinSystem>();
    }
    public void AddCoins(int amount)
    {
        coins += amount;
        if(coinSystem != null)
            coinSystem.CoinUpdate();
    }
}
