using UnityEngine;
using System.Collections;

public class CharacterHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    protected int currentHealth;
    
    public float damageCooldown = 0.75f;
    public bool isInvulnerable = false;
    
    protected Animator animator;
    protected Rigidbody2D rb;
    protected new Collider2D collider2D;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        if (isInvulnerable)
            return;

        currentHealth -= damage;
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(DamageCooldown());
        }
    }

    protected IEnumerator DamageCooldown()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(damageCooldown);
        isInvulnerable = false;
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " was slain!");
        animator.SetTrigger("Death");
        rb.velocity = Vector2.zero;
        
        rb.bodyType = RigidbodyType2D.Static;
        if (collider2D != null)
            collider2D.enabled = false;
        
        var movement = GetComponent<CharacterMovement>();
        if (movement != null)
            movement.enabled = false;
        
        var combat = GetComponent<CharacterCombat>();
        if (combat != null)
            combat.enabled = false;
        
        this.enabled = false;
    }
}
