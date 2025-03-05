using UnityEngine;
using System.Collections;

public class BatCombat : CharacterCombat
{
    [Header("Combat Settings")]
    public int damageAmount = 15;
    public float knockbackForce = 5f;
    public float damageCooldown = 1f;
    private float damageTimer;

    void Update()
    {
        if (damageTimer > 0)
            damageTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && damageTimer <= 0)
        {
            Attack();
            Vector2 knockbackDir = (other.transform.position - transform.position).normalized;
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
            }
            damageTimer = damageCooldown;
        }
    }

    public override void Attack()
    {
        if (GetComponent<CharacterMovement>().isAttacking)
            return;
        GetComponent<CharacterMovement>().isAttacking = true;
        animator.SetFloat("AttackSpeed", 1f / attackSpeed);
        animator.SetTrigger("Attack");
        StartCoroutine(ResetAttackAfterDelay(attackSpeed));
    }
}
