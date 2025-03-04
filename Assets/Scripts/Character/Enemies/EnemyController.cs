using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyMovement enemyMovement;
    private CharacterHealth characterHealth;
    private CharacterCombat characterCombat;
    public float attackRange = 1f;
    public float attackCooldown = 2f;
    private float nextAttackTime = 0f;
    public Transform target;

    void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        characterHealth = GetComponent<CharacterHealth>();
        characterCombat = GetComponent<CharacterCombat>();
        
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (target == null || characterCombat == null)
            return;
            
        float distance = Vector2.Distance(transform.position, target.position);
        if (distance <= attackRange && Time.time >= nextAttackTime)
        {
            characterCombat.Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }
}
