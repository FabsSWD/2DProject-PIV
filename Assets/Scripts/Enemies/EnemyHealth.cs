using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 50;
    protected int currentHealth;
    protected Animator animator;
    protected Rigidbody2D rb;
    protected new Collider2D collider2D;
    [SerializeField] private EnemyCoinDrop coinDrop;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        coinDrop = GetComponent<EnemyCoinDrop>();
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (animator != null)
            animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
            Die();
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
