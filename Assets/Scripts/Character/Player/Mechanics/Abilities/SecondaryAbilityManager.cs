using UnityEngine;
using System.Collections;

public enum SecondaryAbility { None, Shield, SummonPrefab }

public class SecondaryAbilityManager : MonoBehaviour
{
    public SecondaryAbility ability = SecondaryAbility.None;
    
    [Header("Shield Settings")]
    public float invulDuration = 1f;
    public float shieldCooldown = 15f;
    
    [Header("Summon Settings")]
    public GameObject summonFireball;
    public float summonCooldown = 30f;
    public int maxFireballs = 3;
    public float fireballRechargeTime = 40f;
    public Transform AttackPoint;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private CharacterHealth characterHealth;
    private float currentShieldCooldown = 0f;
    private int currentFireballs;
    private bool isRechargingFireball = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
        characterHealth = GetComponent<CharacterHealth>();
        currentFireballs = maxFireballs;
        
        if(GameManager.Instance != null)
            ability = GameManager.Instance.ability;

        ApplyGameManagerValues();
    }

    public void ApplyGameManagerValues()
    {
        if (GameManager.Instance != null)
        {
            ability = GameManager.Instance.ability;
        }
    }


    void Update()
    {
        if (currentShieldCooldown > 0)
            currentShieldCooldown -= Time.deltaTime;
        if (Input.GetButtonDown("Fire2"))
        {
            switch (ability)
            {
                case SecondaryAbility.None:
                    break;
                case SecondaryAbility.Shield:
                    if (currentShieldCooldown <= 0)
                    {
                        StartCoroutine(Shield());
                        currentShieldCooldown = shieldCooldown;
                    }
                    break;
                case SecondaryAbility.SummonPrefab:
                    if (currentFireballs > 0)
                    {
                        Summon();
                        currentFireballs--;
                        if (!isRechargingFireball)
                            StartCoroutine(RechargeFireball());
                    }
                    break;
            }
        }
    }

    IEnumerator Shield()
    {
        if (characterHealth != null)
            characterHealth.isInvulnerable = true;
        if (spriteRenderer != null)
            spriteRenderer.color = Color.blue;
        yield return new WaitForSeconds(invulDuration);
        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;
        if (characterHealth != null)
            characterHealth.isInvulnerable = false;
    }

    void Summon()
    {
        if (summonFireball != null && AttackPoint != null)
            Instantiate(summonFireball, AttackPoint.position, Quaternion.identity);
    }

    IEnumerator RechargeFireball()
    {
        isRechargingFireball = true;
        yield return new WaitForSeconds(fireballRechargeTime);
        if (currentFireballs < maxFireballs)
            currentFireballs++;
        if (currentFireballs < maxFireballs)
            StartCoroutine(RechargeFireball());
        else
            isRechargingFireball = false;
    }
}
