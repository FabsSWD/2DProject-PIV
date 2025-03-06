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
    public GameObject summonPrefab;
    public float summonCooldown = 30f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private CharacterHealth characterHealth;

    private float currentShieldCooldown = 0f;
    private float currentSummonCooldown = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
        
        characterHealth = GetComponent<CharacterHealth>();
    }

    void Update()
    {
        if (currentShieldCooldown > 0)
            currentShieldCooldown -= Time.deltaTime;
        if (currentSummonCooldown > 0)
            currentSummonCooldown -= Time.deltaTime;

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
                    if (currentSummonCooldown <= 0)
                    {
                        Summon();
                        currentSummonCooldown = summonCooldown;
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
        if (summonPrefab != null)
            Instantiate(summonPrefab, transform.position, Quaternion.identity);
    }
}
