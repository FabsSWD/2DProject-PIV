using UnityEngine;
using Pathfinding;

public class BatMovement : CharacterMovement
{
    public enum EnemyState { Patrol, Combat }
    public EnemyState currentState = EnemyState.Patrol;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float nextWaypointDistance = 0.5f;
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;

    [Header("Pathfinding")]
    public Seeker seeker;
    public Path path;
    private int currentWaypoint = 0;

    [Header("Detection")]
    public Transform player;
    public float detectionRadius = 15f;
    public float disengageTime = 2f;
    private float disengageTimer;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (seeker == null)
            seeker = GetComponent<Seeker>();
    }

    void Start()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            currentState = EnemyState.Combat;
        UpdatePath();
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                if (PlayerInDetectionRange())
                {
                    currentState = EnemyState.Combat;
                    disengageTimer = disengageTime;
                    UpdatePath();
                }
                break;
            case EnemyState.Combat:
                ChasePlayer();
                if (!PlayerInDetectionRange())
                {
                    disengageTimer -= Time.deltaTime;
                    if (disengageTimer <= 0f)
                    {
                        currentState = EnemyState.Patrol;
                        UpdatePath();
                    }
                }
                else
                {
                    disengageTimer = disengageTime;
                }
                break;
        }
        HandleMovement();
    }

    void HandleMovement()
    {
        Vector2 waypoint = (Vector2)path.vectorPath[currentWaypoint];
        Vector2 direction = (waypoint - rb.position).normalized; 

        if (direction.x > 0)
        {
            transform.localScale = new Vector3(4, 4, 1);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-4, 4, 1);
        }
    }

    void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;
        Transform targetPoint = patrolPoints[currentPatrolIndex];
        Vector2 direction = (targetPoint.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
        if (Vector2.Distance(transform.position, targetPoint.position) < nextWaypointDistance)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            UpdatePath();
        }
    }

    void ChasePlayer()
    {
        if (path == null || currentWaypoint >= path.vectorPath.Count)
        {
            UpdatePath();
            return;
        }
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - (Vector2)transform.position).normalized;
        rb.velocity = direction * moveSpeed;
        if (Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
            currentWaypoint++;
    }

    public void UpdatePath()
    {
        if (seeker.IsDone())
        {
            Vector3 targetPos = currentState == EnemyState.Combat ? player.position : (patrolPoints != null && patrolPoints.Length > 0 ? patrolPoints[currentPatrolIndex].position : transform.position);
            seeker.StartPath(transform.position, targetPos, OnPathComplete);
        }
    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    bool PlayerInDetectionRange()
    {
        return Vector2.Distance(transform.position, player.position) <= detectionRadius;
    }
}
