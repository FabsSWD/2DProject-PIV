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

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        coinDrop = GetComponent<EnemyCoinDrop>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (animator != null)
            animator.SetTrigger("Hurt");

        if (spriteRenderer != null)
            StartCoroutine(FlashRed());

        if (currentHealth <= 0)
            Die();
    }

    protected IEnumerator FlashRed()
    {
        spriteRenderer.color = new Color(1f, 0f, 0f, 1f);
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.color = originalColor;
    }

    protected virtual void Die()
    {
        if (animator != null)
            animator.SetTrigger("Death");
        
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        if (collider2D != null)
            collider2D.enabled = false;
        
        EnemyCombat combat = GetComponent<EnemyCombat>();
        if (combat != null)
            combat.enabled = false;

        FlyingEnemyAI flyingAI = GetComponent<FlyingEnemyAI>();
        if (flyingAI != null)
            flyingAI.enabled = false;
        
        if (coinDrop != null)
            coinDrop.DropCoins(transform);
        
        Destroy(gameObject, 1f);
    }

    public virtual void OnDeathAnimationEnd()
    {
        Destroy(gameObject);
    }
}
