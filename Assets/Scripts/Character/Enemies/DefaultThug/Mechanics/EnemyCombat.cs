using UnityEngine;

public class EnemyCombat : CharacterCombat
{
    public Transform target;
    private EnemyMovement enemyMovement;

    void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        if (enemyMovement == null)
        {
            Debug.LogError("Missing EnemyMovement component on " + gameObject.name);
        }
        if(characterMovement == null)
        {
            Debug.LogError("Missing CharacterMovement component on " + gameObject.name);
        }
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if(player != null)
                target = player.transform;
            else
                Debug.LogError("Player not found in scene!");
        }
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
