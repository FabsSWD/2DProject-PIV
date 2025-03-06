using UnityEngine;
using Pathfinding;

public class BatAI : FlyingEnemyAI
{
    public float swoopSpeedMultiplier = 1.5f;
    public float stopDistance = 0.5f;
    
    protected override void FixedUpdate()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        rb.velocity = direction * moveSpeed * swoopSpeedMultiplier;

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
            currentWaypoint++;

        if (rb.velocity.x >= 0.01f)
            enemyGFX.localScale = new Vector3(1, 1, 1);
        else if (rb.velocity.x <= -0.01f)
            enemyGFX.localScale = new Vector3(-1, 1, 1);

        float distanceToTarget = Vector2.Distance(rb.position, target.position);
        if (distanceToTarget <= stopDistance)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }
    }
}
