using UnityEngine;
using System.Collections;

public class EnemyCombat : MonoBehaviour
{
    [Header("Combat")]
    public float attackSpeed = 1f;
    public float attackRange = 0.5f;
    public int attackDamage = 10;
    public Transform attackPoint;
    public LayerMask playerLayer;

    protected Animator animator;
    protected Rigidbody2D rb;
    protected bool isAttacking = false;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void Attack()
    {
        if (isAttacking)
            return;
        
        isAttacking = true;
        animator.SetFloat("AttackSpeed", 1f / attackSpeed);
        animator.SetTrigger("Attack");
        StartCoroutine(ResetAttackAfterDelay(attackSpeed));
    }

    protected IEnumerator ResetAttackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnAttackAnimationEnd();
    }

    public virtual void OnAttackAnimationEnd()
    {
        isAttacking = false;
    }

    public virtual void OnCastedHit()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        foreach (Collider2D player in hitPlayers)
        {
            CharacterHealth health = player.GetComponentInParent<CharacterHealth>();
            if (health != null)
            {
                health.TakeDamage(attackDamage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
