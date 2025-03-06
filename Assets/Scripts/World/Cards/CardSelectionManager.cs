using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CardSelectionManager : MonoBehaviour
{
    [System.Serializable]
    public struct CardData
    {
        public CardRarity rarity;
        public int bonusIndex;
        public CardData(CardRarity rarity, int bonusIndex)
        {
            this.rarity = rarity;
            this.bonusIndex = bonusIndex;
        }
    }

    public Card[] cards;
    private List<CardData> cardPool = new List<CardData>();
    private bool cardChosen = false;

    void Awake()
    {
        for (int i = 0; i < 3; i++)
            cardPool.Add(new CardData(CardRarity.Common, i));
        for (int i = 0; i < 4; i++)
            cardPool.Add(new CardData(CardRarity.Rare, i));
        for (int i = 0; i < 2; i++)
            cardPool.Add(new CardData(CardRarity.Epic, i));
    }

    void Start()
    {
        foreach (Card card in cards)
        {
            if (cardPool.Count == 0)
                break;
            int randIndex = Random.Range(0, cardPool.Count);
            CardData selected = cardPool[randIndex];
            card.SetupFromData(this, selected.rarity, selected.bonusIndex);
            cardPool.RemoveAt(randIndex);
        }
    }

    void Update()
        {
            if(cardChosen)
                return;
            
            if(Input.GetButtonDown("Fire1"))
            {
                if(cards.Length > 0 && cards[0] != null)
                    cards[0].AcceptCard();
            }
            else if(Input.GetButtonDown("Fire2"))
            {
                if(cards.Length > 1 && cards[1] != null)
                    cards[1].AcceptCard();
            }
            else if(Input.GetButtonDown("Fire3"))
            {
                if(cards.Length > 2 && cards[2] != null)
                    cards[2].AcceptCard();
            }
        }

    public void CardSelected(Card selectedCard)
    {
        cardChosen = true;
        foreach(Card card in cards)
        {
            if(card != null && card != selectedCard)
                Destroy(card.gameObject);
        }
        Invoke("LoadNextLevel", 1f);
    }

    void LoadNextLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        int levelNumber;
        if(int.TryParse(currentSceneName.Replace("LevelUp", ""), out levelNumber))
            SceneManager.LoadScene("Level" + (levelNumber + 1));
        else
            Debug.LogWarning("Scene name format invalid.");
    }
}
