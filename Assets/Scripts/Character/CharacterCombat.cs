using UnityEngine;
using System.Collections;

public class CharacterCombat : MonoBehaviour
{
    [Header("Combat")]
    public float attackSpeed = 0.5f;
    public float attackRange = 0.5f;
    public int attackDamage = 25;
    public Transform attackPoint;
    public LayerMask enemyLayers;

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

    private IEnumerator ResetAttackAfterDelay(float delay)
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
            Debug.Log("Detected target: " + target.name);
            CharacterHealth health = target.GetComponentInParent<CharacterHealth>();
            if (health != null)
            {
                Debug.Log("Applying damage to: " + target.name);
                health.TakeDamage(attackDamage);
            }
            else
            {
                Debug.Log("No CharacterHealth found on: " + target.name);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
