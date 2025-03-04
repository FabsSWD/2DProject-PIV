using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultEnemy : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHealth;

    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D collider2D;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();

        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("An Enemy was slain! ");
        
        animator.SetTrigger("Death");
        rb.velocity = Vector2.zero;
        collider2D.enabled = false;
        
        this.enabled = false;
    }
}
