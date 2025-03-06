using UnityEngine;
using System.Collections;

public class PlayerMovement : CharacterMovement
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float jumpReleaseMultiplier = 0.5f;
    public int maxJumps = 2;

    public float dashForce = 10f;
    public float dashCooldown = 15f;
    public int maxDash = 2;

    private int jumpCount = 0;
    private int currentDash;
    private bool isDashing = false;
    private bool isRechargingDash = false;

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
        currentDash = maxDash;
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
        GameManager.Instance.moveSpeed = moveSpeed;
        GameManager.Instance.maxJumps = maxJumps;

        if (isAttacking)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (isDashing)
            return;

        if (Input.GetButtonDown("Fire3") && currentDash > 0)
        {
            currentDash--;
            StartCoroutine(Dash());
            if(!isRechargingDash)
                StartCoroutine(RechargeDash());
            return;
        }

        // Salto
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

    IEnumerator Dash()
    {
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        
        float dashDirection = Input.GetAxis("Horizontal");
        if (Mathf.Approximately(dashDirection, 0))
        {
            dashDirection = (transform.localScale.x == -1) ? 1f : -1f;
        }
        
        rb.velocity = new Vector2(dashForce * dashDirection, 0);
        yield return new WaitForSeconds(0.2f);
        rb.gravityScale = originalGravity;
        isDashing = false;
    }

    IEnumerator RechargeDash()
    {
        isRechargingDash = true;
        yield return new WaitForSeconds(dashCooldown);
        if (currentDash < maxDash)
            currentDash++;
        if (currentDash < maxDash)
            StartCoroutine(RechargeDash());
        else
            isRechargingDash = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
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
