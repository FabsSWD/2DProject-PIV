using UnityEngine;

public class PlayerMovement : CharacterMovement
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float jumpReleaseMultiplier = 0.5f;
    public int maxJumps = 2;

    private int jumpCount = 0;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if(GameManager.Instance != null)
        {
            moveSpeed = GameManager.Instance.moveSpeed;
            maxJumps = GameManager.Instance.maxJumps;
        }
        ApplyGameManagerValues();
    }

    public void ApplyGameManagerValues()
    {
        if (GameManager.Instance != null)
        {
            moveSpeed = GameManager.Instance.moveSpeed;
            maxJumps = GameManager.Instance.maxJumps;
        }
    }


    void Update()
    {
        if (isAttacking)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetTrigger("Jump");
            jumpCount++;
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpReleaseMultiplier);
        }

        HandleMovement();
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(1, 1, 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")){
            isGrounded = true;
            jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }
}
