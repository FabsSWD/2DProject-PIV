using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class EnemyMovement : CharacterMovement
{
    public Transform target;
    public float updateRate = 2f;
    private Seeker seeker;
    private Path path;
    public float speed = 5f;
    public float nextWaypointDistance = 3f;
    private int currentWaypoint = 0;

    [Header("Jump")]
    public float jumpForce = 5f;
    public LayerMask groundLayer;

    void Start()
    {
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
            seeker.StartPath(GetComponent<Rigidbody2D>().position, target.position, OnPathComplete);
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

    void FixedUpdate()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
            return;

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - GetComponent<Rigidbody2D>().position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Force);

        if (currentWaypoint < path.vectorPath.Count - 1)
        {
            float diffy = path.vectorPath[currentWaypoint + 1].y - path.vectorPath[currentWaypoint].y;
            if (diffy > 0.5f && IsGrounded())
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        float distance = Vector2.Distance(GetComponent<Rigidbody2D>().position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
            currentWaypoint++;
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.2f, Vector2.down, 0.1f, groundLayer);
        return hit.collider != null;
    }
}
