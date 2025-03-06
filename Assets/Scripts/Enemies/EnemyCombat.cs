using UnityEngine;
using System.Collections;

public class EnemyCombat : MonoBehaviour
{
    public int attackDamage = 10;
    public float attackCooldown = 1.5f;
    private bool canAttack = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canAttack)
        {
            Attack(other);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canAttack)
        {
            Attack(other);
        }
    }

    private void Attack(Collider2D playerCollider)
    {
        CharacterHealth playerHealth = playerCollider.GetComponentInParent<CharacterHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            canAttack = false;
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    private void ResetAttack()
    {
        canAttack = true;
    }
}
