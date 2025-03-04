using UnityEngine;

public abstract class CharacterMovement : MonoBehaviour
{
    [HideInInspector]
    public bool isAttacking = false;
    [HideInInspector]
    public bool isGrounded = false;
}
