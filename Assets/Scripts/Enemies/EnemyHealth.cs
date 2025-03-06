using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 50;
    protected int currentHealth;
    protected Animator animator;
    protected Rigidbody2D rb;
    protected new Collider2D collider2D;
    [SerializeField] private EnemyCoinDrop coinDrop;
    protected SpriteRenderer spriteRenderer;
    protected Color originalColor;
    
    private WaveManager waveManager;
    protected bool isDead = false;
    public bool isInvulnerable = false;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        coinDrop = GetComponent<EnemyCoinDrop>();
        waveManager = FindObjectOfType<WaveManager>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        if (isInvulnerable || isDead)
            return;
            
        currentHealth -= damage;
        if (animator != null)
            animator.SetTrigger("Hurt");

        if (spriteRenderer != null)
            StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            isInvulnerable = true;
            Die();
        }
    }

    protected IEnumerator FlashRed()
    {
        spriteRenderer.color = new Color(1f, 0f, 0f, 1f);
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.color = originalColor;
    }

    protected virtual void Die()
    {
        if (isDead)
            return;
        isDead = true;
        
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }
        
        if (animator != null)
            animator.SetTrigger("Death");

        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        
        EnemyCombat combat = GetComponent<EnemyCombat>();
        if (combat != null)
            combat.enabled = false;

        FlyingEnemyAI flyingAI = GetComponent<FlyingEnemyAI>();
        if (flyingAI != null)
            flyingAI.enabled = false;
        
        GroundEnemyAI groundEnemyAI = GetComponent<GroundEnemyAI>();
        if (groundEnemyAI != null)
            groundEnemyAI.enabled = false;
        
        if (coinDrop != null)
            coinDrop.DropCoins(transform);
        
        if (waveManager != null)
            waveManager.OnEnemyKilled();

        Destroy(gameObject, 1f);
    }

    public virtual void OnDeathAnimationEnd()
    {
        Destroy(gameObject);
    }
}
