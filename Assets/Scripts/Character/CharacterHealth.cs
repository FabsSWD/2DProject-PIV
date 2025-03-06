using UnityEngine;
using System.Collections;

public class CharacterHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;
    
    public float damageCooldown = 0.75f;
    public bool isInvulnerable = false;
    
    protected Animator animator;
    protected Rigidbody2D rb;
    protected new Collider2D collider2D;
    protected HealthSystem healthSystem;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        currentHealth = maxHealth;

        healthSystem = FindObjectOfType<HealthSystem>();
        if (healthSystem == null)
        {
            if (HealthSystem.Instance != null)
            {
                healthSystem = HealthSystem.Instance;
            }
        }
    }

    void Start()
    {
        if(GameManager.Instance != null)
        {
            maxHealth = GameManager.Instance.maxHealth;
            currentHealth = GameManager.Instance.currentHealth;
        }

        ApplyGameManagerValues();
    }

    void Update()
    {
        GameManager.Instance.maxHealth = maxHealth;
        GameManager.Instance.currentHealth = currentHealth;
    }

    public void ApplyGameManagerValues()
    {
        if (GameManager.Instance != null)
        {
            maxHealth = GameManager.Instance.maxHealth;
            currentHealth = GameManager.Instance.currentHealth;
        }
    }


    public virtual void TakeDamage(int damage)
    {
        if (isInvulnerable)
            return;

        currentHealth -= damage;
        if (animator != null)
            animator.SetTrigger("Hurt");

        if (healthSystem != null)
            healthSystem.UpdateGraphics();

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
        if (animator != null)
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

        if (GameManager.Instance != null)
        {
            GameManager.Instance.TriggerGameOver();
        }
    }
}
