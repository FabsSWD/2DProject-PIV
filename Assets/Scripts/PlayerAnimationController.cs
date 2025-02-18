using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    // Referencia al script de movimiento
    private PlayerMovement playerMovement;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        // Asumiendo que PlayerMovement está en el mismo GameObject:
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // Actualizamos el parámetro "Grounded" directamente desde PlayerMovement
        animator.SetBool("Grounded", playerMovement.isGrounded);
        // Usamos la velocidad vertical para el parámetro "airspeed"
        animator.SetFloat("airspeed", rb.velocity.y);

        // Por ejemplo, para determinar el estado de animación basado en la velocidad y si está en el suelo:
        int animState = 0;  // 0 = Idle
        if (!playerMovement.isGrounded && rb.velocity.y < 0)
        {
            animState = 3;  // Jump (caída)
        }
        else if (playerMovement.isGrounded)
        {
            float horizontal = Input.GetAxis("Horizontal");
            animState = (Mathf.Abs(horizontal) > 0.1f) ? 2 : 0;  // 2 = Run, 0 = Idle
        }
        animator.SetInteger("AnimState", animState);

        // Puedes seguir controlando los triggers de Attack, Hurt, Death, etc. según tu lógica.
        if (Input.GetButtonDown("Jump") && playerMovement.isGrounded)
        {
            animator.SetTrigger("Jump");
        }
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");
        }
    }
}
