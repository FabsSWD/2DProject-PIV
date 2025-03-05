using System;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;
    private EnemyMovement enemyMovement;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        enemyMovement = GetComponent<EnemyMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        bool grounded = enemyMovement.IsGrounded();
        animator.SetBool("Grounded", grounded);

        if (enemyMovement.path == null || enemyMovement.path.vectorPath.Count == 0)
        {
            animator.SetInteger("AnimState", 0); 
            return;
        }

        if (enemyMovement.currentWaypoint < 0 || enemyMovement.currentWaypoint >= enemyMovement.path.vectorPath.Count)
        {
            animator.SetInteger("AnimState", 0);
            return;
        }

        Vector2 waypoint = (Vector2)enemyMovement.path.vectorPath[enemyMovement.currentWaypoint];
        Vector2 direction = (waypoint - rb.position).normalized; 

        int animState = 0;
        
        if (!grounded)
        {
            animState = 3;
        }
        else if (Mathf.Abs(direction.x) > 0.1f) 
        {
            animState = 2;
        }
        else
        {
            animState = 0;
        }

        animator.SetInteger("AnimState", animState);

        if (direction.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
