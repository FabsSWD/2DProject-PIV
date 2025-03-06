using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatHealth : EnemyHealth
{
    protected override void Die()
    {
        base.Die();
        GetComponentInChildren<CircleCollider2D>().enabled = false;
        GetComponentInChildren<BoxCollider2D>().enabled = false;
    }
}