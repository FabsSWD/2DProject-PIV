using UnityEngine;

public class GroundEnemyAI : MonoBehaviour
{
    public Transform[] waypoints; // Patrol waypoints
    public float patrolSpeed = 2f;
    public float chaseSpeed = 3.5f;
    public float detectionRange = 5f;
    public float attackRange = 1f;
    public float attackCooldown = 1.5f;

    protected Transform player;
    protected int currentWaypointIndex = 0;
    protected bool isChasing = false;
    protected bool canAttack = true;
    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && canAttack)
        {
            Attack();
        }
        else if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }
    }

    protected virtual void FixedUpdate()
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

    protected virtual void Patrol()
    {
        if (waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector2 direction = (targetWaypoint.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * patrolSpeed, rb.velocity.y);

        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }

        FlipSprite(direction.x);
    }

    protected virtual void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * chaseSpeed, rb.velocity.y);

        FlipSprite(direction.x);
    }

    protected virtual void Attack()
    {
        Debug.Log("Enemy attacks player!");
        canAttack = false;
        rb.velocity = Vector2.zero;

        // Simulate an attack delay (Can be replaced with an actual attack animation)
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    protected virtual void ResetAttack()
    {
        canAttack = true;
    }

    protected virtual void FlipSprite(float directionX)
    {
        if (directionX > 0)
            spriteRenderer.flipX = false;
        else if (directionX < 0)
            spriteRenderer.flipX = true;
    }
}