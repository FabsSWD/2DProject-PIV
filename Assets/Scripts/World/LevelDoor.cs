using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class LevelDoor : MonoBehaviour
{
    public String nextLevel;
    public int price;
    public TMP_Text messageText;
    public GameObject textBackground;
    private PlayerInventory playerInventory;
    private bool playerInside = false;

    void Start()
    {
        textBackground.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInventory = other.GetComponent<PlayerInventory>();
            playerInside = true;
            textBackground.SetActive(true);
            UpdateMessage();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            messageText.text = "";
            textBackground.SetActive(false);
            playerInventory = null;
        }
    }

    void Update()
    {
        if (!playerInside || playerInventory == null)
            return;

        UpdateMessage();

        if (Input.GetButtonDown("Submit"))
        {
            if (playerInventory.coins >= price && playerInventory.SpendCoins(price))
            {
                SceneManager.LoadScene(nextLevel);
            }
        }
    }

    void UpdateMessage()
    {
        if (playerInventory.coins < price)
            messageText.text = "You need " + price + " coins to open the door.";
        else
            messageText.text = "Open the door (Price " + price + " coins). Press Submit.";
    }
}
