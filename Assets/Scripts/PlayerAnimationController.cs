using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private PlayerMovement playerMovement;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        animator.SetBool("Grounded", playerMovement.isGrounded);
        animator.SetFloat("airspeed", rb.velocity.y);
        animator.SetFloat("AttackSpeed", 1f / playerMovement.attackSpeed);

        int animState = 0;  // 0 = Idle, 2 = Run, 3 = Jump
        if (!playerMovement.isGrounded)
        {
            animState = 3;
        }
        else
        {
            float horizontal = Input.GetAxis("Horizontal");
            animState = (Mathf.Abs(horizontal) > 0.1f) ? 2 : 0;
        }
        animator.SetInteger("AnimState", animState);

        if (Input.GetButtonDown("Jump"))
        {
            animator.SetTrigger("Jump");
        }
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");
        }
    }
}
