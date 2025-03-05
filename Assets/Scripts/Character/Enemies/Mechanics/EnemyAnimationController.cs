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

        // Verificar que el path y el waypoint sean válidos
        if (enemyMovement.path == null || enemyMovement.path.vectorPath.Count == 0)
        {
            animator.SetInteger("AnimState", 0); // Idle si no hay camino
            return;
        }

        // Obtener la dirección real basándose en el siguiente waypoint
        Vector2 waypoint = (Vector2)enemyMovement.path.vectorPath[enemyMovement.currentWaypoint];
        Vector2 direction = (waypoint - rb.position).normalized; 

        int animState = 0; // 0 = Idle, 2 = Run, 3 = Jump

        // Determinar el estado de animación
        if (!grounded)
        {
            animState = 3; // En el aire
        }
        else if (Mathf.Abs(direction.x) > 0.1f) 
        {
            animState = 2; // Corriendo si hay movimiento en X
        }
        else
        {
            animState = 0; // Idle
        }

        animator.SetInteger("AnimState", animState);

        // **Voltear el sprite según la dirección**
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Mirar a la derecha
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Mirar a la izquierda
        }
    }
}
