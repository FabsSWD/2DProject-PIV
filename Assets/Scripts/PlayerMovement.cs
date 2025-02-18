using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Velocidad de movimiento y fuerza de salto
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    // Componentes
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    // Variable para controlar si está en el suelo
    public bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput > 0)
            spriteRenderer.flipX = true;
        else if (moveInput < 0)
            spriteRenderer.flipX = false;
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
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
