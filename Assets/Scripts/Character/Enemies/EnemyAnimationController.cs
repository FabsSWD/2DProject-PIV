using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;
    private EnemyMovement enemyMovement;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        enemyMovement = GetComponent<EnemyMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        animator.SetBool("Grounded", enemyMovement.IsGrounded());
        
        float horizontalSpeed = Mathf.Abs(rb.velocity.x);
        int animState = 0; // Idle
        if (!enemyMovement.IsGrounded())
            animState = 3; // Jump
        else
            animState = (horizontalSpeed > 0.1f) ? 2 : 0; 
        
        animator.SetInteger("AnimState", animState);
    }
}
