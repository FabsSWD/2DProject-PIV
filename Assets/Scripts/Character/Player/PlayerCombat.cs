using UnityEngine;

public class PlayerCombat : CharacterCombat
{
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && GetComponent<PlayerMovement>().isGrounded && !GetComponent<PlayerMovement>().isAttacking)
        {
            Attack();
            return;
        }
    }
}
