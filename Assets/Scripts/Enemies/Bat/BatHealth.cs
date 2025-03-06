using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatHealth : EnemyHealth
{
    // When enemy dies, deactivates the hitbox collider
    protected override void Die()
    {
        base.Die();
        GetComponentInChildren<CircleCollider2D>().enabled = false;
        GetComponentInChildren<CapsuleCollider2D>().enabled = false;
    }
}
