using UnityEngine;

public class RatAI : GroundEnemyAI
{
    public float scurrySpeedMultiplier = 1.2f; // Increases speed for quick movement
    public bool canJumpOverObstacles = false;
    public float jumpForce = 5f;

    protected override void FixedUpdate()
    {
        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    protected override void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * chaseSpeed * scurrySpeedMultiplier, rb.velocity.y);

        FlipSprite(direction.x);

        if (canJumpOverObstacles)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * Mathf.Sign(direction.x), 1f, LayerMask.GetMask("Obstacle"));
            if (hit.collider != null)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }
    }
}