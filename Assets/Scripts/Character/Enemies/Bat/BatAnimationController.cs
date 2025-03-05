using UnityEngine;

public class BatAnimationController : MonoBehaviour
{
    [Header("Animation Settings")]
    private Animator animator;
    private Rigidbody2D rb;
    private BatMovement batMovement;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        batMovement = GetComponent<BatMovement>();
    }

    void Update()
    {
        animator.SetFloat("Speed", rb.velocity.magnitude);
        animator.SetBool("IsAttacking", batMovement.isAttacking);
    }
}
