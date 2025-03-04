using UnityEngine;

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
        // Check if character is not already attacking
        if (characterMovement != null && characterMovement.isAttacking)
            return;
        characterMovement.isAttacking = true;
        animator.SetFloat("AttackSpeed", 1f / attackSpeed);
        animator.SetTrigger("Attack");
    }

    public virtual void OnAttackAnimationEnd()
    {
        if (characterMovement != null)
            characterMovement.isAttacking = false;
    }

    public virtual void OnCastedHit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            CharacterHealth enemyHealth = enemy.GetComponent<CharacterHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
