using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int coins = 0;
    
    public void AddCoins(int amount)
    {
        coins += amount;
    }
}
