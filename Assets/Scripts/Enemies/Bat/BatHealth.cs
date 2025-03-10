using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatHealth : EnemyHealth
{
    protected override void Die()
    {
        base.Die();
        GetComponentInChildren<CircleCollider2D>().enabled = false;
        GetComponentInChildren<CapsuleCollider2D>().enabled = false;
    }
}
