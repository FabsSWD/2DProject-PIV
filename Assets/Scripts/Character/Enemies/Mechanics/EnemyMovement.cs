using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class EnemyMovement : CharacterMovement
{
    public Transform target;
    public float updateRate = 2f;
    private Seeker seeker;
    public Path path;
    public float speed = 5f;               
    public float nextWaypointDistance = 3f;
    public int currentWaypoint = 0;
    public float stopDistance = 1.5f;

    [Header("Jump")]
    public float jumpForce = 5f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();

        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        InvokeRepeating("UpdatePath", 0f, 1f / updateRate);
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && target != null)
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void Update()
    {
        if (target == null)
            return;
        
        float distanceToTarget = Vector2.Distance(rb.position, target.position);
        
        if (distanceToTarget <= stopDistance)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }
        
        if (path == null || currentWaypoint >= path.vectorPath.Count)
            return;
        
        Vector2 waypoint = (Vector2)path.vectorPath[currentWaypoint];
        Vector2 direction = (waypoint - rb.position).normalized;
        
        HandleMovement(direction);
        
        if (currentWaypoint < path.vectorPath.Count - 1)
        {
            float diffY = path.vectorPath[currentWaypoint + 1].y - waypoint.y;
            if (diffY > 0.5f && IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }
        
        float distance = Vector2.Distance(rb.position, waypoint);
        if (distance < nextWaypointDistance)
            currentWaypoint++;
    }

    void HandleMovement(Vector2 direction)
    {
        float moveInput = direction.x;
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (moveInput > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(1, 1, 1);
    }

    public bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.2f, Vector2.down, 0.1f, groundLayer);
        return hit.collider != null;
    }
}
