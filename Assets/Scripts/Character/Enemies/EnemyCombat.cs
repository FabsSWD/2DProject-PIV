using UnityEngine;

public class EnemyCombat : CharacterCombat
{
    public Transform target;
    private EnemyMovement enemyMovement;

    void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (target == null)
            return;
        
        float distance = Vector2.Distance(transform.position, target.position);
        
        if (distance <= attackRange && enemyMovement.IsGrounded() && !characterMovement.isAttacking)
        {
            Attack();
            return;
        }
    }
}
