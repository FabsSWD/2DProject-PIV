using UnityEngine;
using UnityEngine.UI;
public class CoinSystem : MonoBehaviour
{
    private PlayerInventory playerInventory;
    public Text currentCoinsText;
    void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        currentCoinsText.text = playerInventory.coins.ToString();
    }
    public void CoinUpdate()
    {
        currentCoinsText.text = playerInventory.coins.ToString();
    }
}
