using UnityEngine;
using System.Collections;

public class CharacterCombat : MonoBehaviour
{
    [Header("Combat")]
    public float attackSpeed = 0.5f;
    public float attackRange = 0.5f;
    public int attackDamage = 0;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public float knockbackForce = 5f;

    protected Animator animator;
    protected Rigidbody2D rb;
    protected CharacterMovement characterMovement;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        characterMovement = GetComponent<CharacterMovement>();
    }

    public virtual void Attack()
    {
        if (characterMovement != null && characterMovement.isAttacking)
            return;
        characterMovement.isAttacking = true;
        animator.SetFloat("AttackSpeed", 1f / attackSpeed);
        animator.SetTrigger("Attack");
        StartCoroutine(ResetAttackAfterDelay(attackSpeed));
    }

    public IEnumerator ResetAttackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnAttackAnimationEnd();
    }

    public virtual void OnAttackAnimationEnd()
    {
        if (characterMovement != null)
            characterMovement.isAttacking = false;
    }

    public virtual void OnCastedHit()
    {
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D target in hitTargets)
        {
            EnemyHealth health = target.GetComponentInParent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(attackDamage);
                Rigidbody2D enemyRb = target.GetComponent<Rigidbody2D>();
                if (enemyRb == null)
                    enemyRb = target.GetComponentInParent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    Vector2 knockbackDirection = (target.transform.position - transform.position).normalized;
                    enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
