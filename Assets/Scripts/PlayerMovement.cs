using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    
    public float attackSpeed = 1f;
    public bool isAttacking = false;
    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    
    public bool isGrounded = false;
    
    private const float baseAttackDuration = 0.7f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        if(isAttacking)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        
        if(Input.GetButtonDown("Fire1") && isGrounded && !isAttacking)
        {
            isAttacking = true;
            StartCoroutine(AttackRoutine());
            return;
        }
        
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
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
    
    IEnumerator AttackRoutine()
    {
        Animator animator = GetComponent<Animator>();
        float multiplier = baseAttackDuration * attackSpeed;
        float timeout = multiplier - (multiplier * 0.005f);

        animator.SetFloat("AttackSpeed", multiplier);
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(timeout);
        
        isAttacking = false;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }
}
