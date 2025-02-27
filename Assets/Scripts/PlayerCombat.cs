using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    
    public float attackSpeed = 1f;
    public float attackRange = 0.5f;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    private Rigidbody2D rb;
    private Animator animator;
    private PlayerMovement playerMovement;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }    
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && playerMovement.isGrounded && !playerMovement.isAttacking)
        {
            Attack();
            return;
        }
    }

    void Attack()
    {
        playerMovement.isAttacking = true;
        animator.SetFloat("AttackSpeed", 1f / attackSpeed);
        animator.SetTrigger("Attack");
    }

    public void OnAttackAnimationEnd()
    {
        playerMovement.isAttacking = false;
    }

    public void OnCastedHit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        
        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hitted " + enemy.name);
        }        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
