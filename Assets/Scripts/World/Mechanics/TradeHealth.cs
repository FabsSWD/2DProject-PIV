using UnityEngine;
using TMPro;
using System.Collections;

public enum TradeType { GoldForHealth, HealthForGold }

public class TradeHealth : MonoBehaviour
{
    public TradeType tradeType = TradeType.GoldForHealth;
    
    public int goldToTrade = 10;
    public GameObject lifeOrbPrefab;
    public int minLifeOrbs = 1;
    public int maxLifeOrbs = 3;
    
    public int healthToTrade = 10;
    private int currentGoldReward = 0;
    public GameObject coinPrefab;
    public int minCoinDrops = 3;
    public int maxCoinDrops = 7;
    
    public TMP_Text tradeMessageText;
    public float messageDuration = 2f;
    
    public Transform dropPoint;
    
    private bool playerInside = false;

    private HealthSystem healthSystem;

    void Start()
    {
        healthSystem = FindObjectOfType<HealthSystem>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            playerInside = true; 
            if(tradeType == TradeType.HealthForGold)
            {
                currentGoldReward = Random.Range(minCoinDrops, maxCoinDrops + 1);
            }
            UpdateTradeMessage();
            if(tradeMessageText != null)
                tradeMessageText.enabled = true;
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            playerInside = false;
            if(tradeMessageText != null)
            {
                tradeMessageText.text = "";
                tradeMessageText.enabled = false;
            }
        }
    }
    
    void UpdateTradeMessage()
    {
        if(tradeMessageText != null)
        {
            if(tradeType == TradeType.GoldForHealth)
            {
                tradeMessageText.text = "Press Submit to trade " + goldToTrade + " gold for health.";
            }
            else if(tradeType == TradeType.HealthForGold)
            {
                tradeMessageText.text = "Press Submit to trade " + healthToTrade + " health for gold.";
            }
        }
    }
    
    void Update()
    {
        if(playerInside && Input.GetButtonDown("Submit"))
        {
            PerformTrade();
        }
    }
    
    public void PerformTrade()
    {
        PlayerInventory playerInv = FindObjectOfType<PlayerInventory>();
        CharacterHealth playerHealth = FindObjectOfType<CharacterHealth>();
        if(playerInv != null && playerHealth != null)
        {
            if(tradeType == TradeType.GoldForHealth)
            {
                if(playerInv.coins >= goldToTrade)
                {
                    bool spent = playerInv.SpendCoins(goldToTrade);
                    if(spent)
                    {
                        int orbCount = Random.Range(minLifeOrbs, maxLifeOrbs + 1);
                        for(int i = 0; i < orbCount; i++)
                        {
                            Instantiate(lifeOrbPrefab, dropPoint.position, Quaternion.identity);
                        }
                        StartCoroutine(ShowMessage(goldToTrade + " gold exchanged for " + orbCount + " health orbs."));
                    }
                    else
                    {
                        StartCoroutine(ShowMessage("Unable to spend gold."));
                    }
                }
                else
                {
                    StartCoroutine(ShowMessage("Not enough gold! " + goldToTrade + " gold required."));
                }
            }
            else if(tradeType == TradeType.HealthForGold)
            {
                if(playerHealth.currentHealth > healthToTrade)
                {
                    playerHealth.currentHealth = Mathf.Max(playerHealth.currentHealth - healthToTrade, 0);
                    int coinDropCount = Random.Range(minCoinDrops, maxCoinDrops + 1);
                    for(int i = 0; i < coinDropCount; i++)
                    {
                        Instantiate(coinPrefab, dropPoint.position, Quaternion.identity);
                    }
                    StartCoroutine(ShowMessage(healthToTrade + " health traded for " + currentGoldReward + " gold and " + coinDropCount + " coin drops."));
                    healthSystem.UpdateGraphics();
                }
                else
                {
                    StartCoroutine(ShowMessage("Not enough health to trade!"));
                }
            }
        }
        else
        {
            Debug.LogWarning("PlayerInventory or CharacterHealth not found in the scene.");
        }
    }
    
    IEnumerator ShowMessage(string message)
    {
        if(tradeMessageText != null)
        {
            tradeMessageText.text = message;
            tradeMessageText.enabled = true;
            yield return new WaitForSeconds(messageDuration);
            if(playerInside)
            {
                UpdateTradeMessage();
            }
            else
            {
                tradeMessageText.text = "";
                tradeMessageText.enabled = false;
            }
        }
        yield break;
    }
}
