using UnityEngine;
using System.Collections;

public enum SecondaryAbility { None, Shield, SummonPrefab, Explosion }

public class SecondaryAbilityManager : MonoBehaviour
{
    public SecondaryAbility ability = SecondaryAbility.None;
    public LayerMask affectedLayers;
    
    [Header("Shield Settings")]
    public float invulDuration = 1f;
    public float shieldCooldown = 15f;
    
    [Header("Summon Settings")]
    public GameObject summonPrefab;
    public float summonCooldown = 30f;
    
    [Header("Explosion Settings")]
    public GameObject explosionPrefab;
    public float explosionCooldown = 45f;
    public int explosionDamage = 250;
    public float effectRadius = 5f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private CharacterHealth characterHealth;

    private float currentShieldCooldown = 0f;
    private float currentSummonCooldown = 0f;
    private float currentExplosionCooldown = 0f;

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
        if (currentExplosionCooldown > 0)
            currentExplosionCooldown -= Time.deltaTime;

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
                case SecondaryAbility.Explosion:
                    if (currentExplosionCooldown <= 0)
                    {
                        CastExplosion();
                        currentExplosionCooldown = explosionCooldown;
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

    void CastExplosion()
    {
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, effectRadius, affectedLayers);
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == gameObject)
                continue;
            
            CharacterHealth health = hit.GetComponent<CharacterHealth>();
            if (health == null)
                health = hit.GetComponentInParent<CharacterHealth>();
            
            if (health != null)
                health.TakeDamage(explosionDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, effectRadius);
    }
}
