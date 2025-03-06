using UnityEngine;

public class PlayerCombat : CharacterCombat
{
    void Start()
    {
        if(GameManager.Instance != null)
            attackDamage = GameManager.Instance.attackDamage;
        
        ApplyGameManagerValues();
    }

    public void ApplyGameManagerValues()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.attackDamage = attackDamage;
        }
    }

    
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && GetComponent<PlayerMovement>().isGrounded && !GetComponent<PlayerMovement>().isAttacking)
        {
            Attack();
            return;
        }
    }
}
