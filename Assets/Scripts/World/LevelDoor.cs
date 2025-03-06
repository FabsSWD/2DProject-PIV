using UnityEngine;
using TMPro;

public class LevelDoor : MonoBehaviour
{
    public int nextLevel;
    public int price;
    public TMP_Text messageText;
    public GameObject textBackground;
    private PlayerInventory playerInventory;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            playerInventory = other.GetComponent<PlayerInventory>();
            UpdateMessage();
            textBackground.SetActive(true);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
            UpdateMessage();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            messageText.text = "";
            textBackground.SetActive(false);
        }
    }

    void UpdateMessage()
    {
        if(playerInventory.coins < price)
            messageText.text = "You need " + price + " coins to open the door.";
        else
            messageText.text = "Open the door (Price " + price + " coins).";
    }
}
