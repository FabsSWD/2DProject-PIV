using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    // Referencia al Animator y Rigidbody2D
    private Animator animator;
    private Rigidbody2D rb;

    // Variable para determinar si el jugador está en el suelo.
    public bool isGrounded = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        animator.SetBool("Grounded", isGrounded);

        animator.SetFloat("airspeed", rb.velocity.y);

        float horizontal = Input.GetAxis("Horizontal");
        int animState = 0;

        if (!isGrounded)
        {
            animState = 3;
        }
        else if (Mathf.Abs(horizontal) > 0.1f)
        {
            animState = 2;
        }
        else
        {
            animState = 0;
        }
        animator.SetInteger("AnimState", animState);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            animator.SetTrigger("Jump");
        }

        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
