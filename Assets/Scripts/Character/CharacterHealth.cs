using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    protected int currentHealth;
    
    protected Animator animator;
    protected Rigidbody2D rb;
    protected Collider2D collider2D;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " was slain!");
        animator.SetTrigger("Death");
        rb.velocity = Vector2.zero;
        if(collider2D != null)
            collider2D.enabled = false;
        this.enabled = false;
    }
}
