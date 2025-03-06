using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public enum CardRarity { Common, Rare, Epic }
public enum CommonBonus { LifePlus5, SpeedMinus0_1, Heal20Percent }
public enum RareBonus { JumpPlus1, MaxLifePlus50Percent, AttackDamagePlus5 }
public enum EpicBonus { Shield, SummonPrefab }

public class Card : MonoBehaviour
{
    public CardRarity rarity;
    public CommonBonus commonBonus;
    public RareBonus rareBonus;
    public EpicBonus epicBonus;
    public GameObject rarityIcon;
    public TMP_Text bonusText;
    private CardSelectionManager selectionManager;

    private CharacterCombat characterCombat;
    private PlayerMovement playerMovement;
    private CharacterHealth characterHealth;
    private PlayerInventory playerInventory;
    private SecondaryAbilityManager secondaryAbility;

    void Start()
    {
        characterCombat = FindObjectOfType<CharacterCombat>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        characterHealth = FindObjectOfType<CharacterHealth>();
        playerInventory = FindObjectOfType<PlayerInventory>();
        secondaryAbility = FindObjectOfType<SecondaryAbilityManager>();    
    }

    public void SetupFromData(CardSelectionManager manager, CardRarity cardRarity, int bonusIndex)
    {
        selectionManager = manager;
        rarity = cardRarity;
        switch(rarity)
        {
            case CardRarity.Common:
                commonBonus = (CommonBonus)bonusIndex;
                bonusText.text = GetCommonBonusDescription(commonBonus);
                if(rarityIcon != null)
                {
                    SpriteRenderer sr = rarityIcon.GetComponent<SpriteRenderer>();
                    if(sr != null)
                        sr.color = Color.grey;
                }
                break;
            case CardRarity.Rare:
                rareBonus = (RareBonus)bonusIndex;
                bonusText.text = GetRareBonusDescription(rareBonus);
                if(rarityIcon != null)
                {
                    SpriteRenderer sr = rarityIcon.GetComponent<SpriteRenderer>();
                    if(sr != null)
                        sr.color = new Color(0.68f, 0.85f, 0.9f);
                }
                break;
            case CardRarity.Epic:
                epicBonus = (EpicBonus)bonusIndex;
                bonusText.text = GetEpicBonusDescription(epicBonus);
                if(rarityIcon != null)
                {
                    SpriteRenderer sr = rarityIcon.GetComponent<SpriteRenderer>();
                    if(sr != null)
                        sr.color = new Color(0.5f, 0f, 0.5f);
                }
                break;
        }
    }

    string GetCommonBonusDescription(CommonBonus bonus)
    {
        switch(bonus)
        {
            case CommonBonus.LifePlus5: return "+5 Life";
            case CommonBonus.SpeedMinus0_1: return "-0.1 Speed";
            case CommonBonus.Heal20Percent: return "Heal 20% Life";
        }
        return "";
    }

    string GetRareBonusDescription(RareBonus bonus)
    {
        switch(bonus)
        {
            case RareBonus.JumpPlus1: return "+1 Jump";
            case RareBonus.MaxLifePlus50Percent: return "+50% Max Life";
            case RareBonus.AttackDamagePlus5: return "+5 Attack Damage";
        }
        return "";
    }

    string GetEpicBonusDescription(EpicBonus bonus)
    {
        switch(bonus)
        {
            case EpicBonus.Shield: return "Gain Shield Ability";
            case EpicBonus.SummonPrefab: return "Gain Summon Ability";
        }
        return "";
    }

    public void AcceptCard()
    {
        switch(rarity)
        {
            case CardRarity.Common:
                ApplyCommonBonus();
                break;
            case CardRarity.Rare:
                ApplyRareBonus();
                break;
            case CardRarity.Epic:
                ApplyEpicBonus();
                break;
        }
        selectionManager.CardSelected(this);
        Destroy(gameObject);
    }

    void ApplyCommonBonus()
    {
        switch(commonBonus)
        {
            case CommonBonus.LifePlus5:
                if(characterHealth != null)
                    characterHealth.AddHealth(5);
                break;
            case CommonBonus.SpeedMinus0_1:
                if(playerMovement != null)
                    playerMovement.ModifyMoveSpeed(-0.1f);
                break;
            case CommonBonus.Heal20Percent:
                if(characterHealth != null)
                {
                    int healAmount = Mathf.RoundToInt(characterHealth.maxHealth * 0.2f);
                    characterHealth.AddHealth(healAmount);
                }
                break;
        }
    }

    void ApplyRareBonus()
    {
        switch(rareBonus)
        {
            case RareBonus.JumpPlus1:
                if(playerMovement != null)
                    playerMovement.AddMaxJumps(1);
                break;
            case RareBonus.MaxLifePlus50Percent:
                if(characterHealth != null)
                {
                    int extra = Mathf.RoundToInt(characterHealth.maxHealth * 0.5f);
                    characterHealth.IncreaseMaxHealth(extra);
                }
                break;
            case RareBonus.AttackDamagePlus5:
                if(characterCombat != null)
                    characterCombat.AddAttackDamage(5);
                break;
        }
    }

    void ApplyEpicBonus()
    {
        switch(epicBonus)
        {
            case EpicBonus.Shield:
                if(secondaryAbility != null)
                    secondaryAbility.SetAbility(SecondaryAbilityManager.SecondaryAbility.Shield);
                break;
            case EpicBonus.SummonPrefab:
                if(secondaryAbility != null)
                    secondaryAbility.SetAbility(SecondaryAbilityManager.SecondaryAbility.SummonPrefab);
                break;
        }
    }
}
