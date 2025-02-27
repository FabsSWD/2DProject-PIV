using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private PlayerMovement playerMovement;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // Actualizamos parámetros basados en el estado del movimiento
        animator.SetBool("Grounded", playerMovement.isGrounded);
        animator.SetFloat("AirSpeed", rb.velocity.y);

        // Definimos el estado de animación: 0 = Idle, 2 = Run, 3 = Jump
        int animState = 0;
        if (!playerMovement.isGrounded)
        {
            animState = 3;
        }
        else
        {
            float horizontal = Input.GetAxis("Horizontal");
            animState = (Mathf.Abs(horizontal) > 0.1f) ? 2 : 0;
        }
        animator.SetInteger("AnimState", animState);

        // Si lo deseas, podrías actualizar aquí otros parámetros (por ejemplo, AttackSpeed) 
        // en caso de no hacerlo desde PlayerMovement.
    }
}
