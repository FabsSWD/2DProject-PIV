using UnityEngine;
using System.Collections;

public class FlyingEnemyCombat : MonoBehaviour
{
    public int attackDamage = 10;
    public float attackInterval = 1f;
    private Coroutine damageCoroutine;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && damageCoroutine == null)
            damageCoroutine = StartCoroutine(DamagePlayer(other));
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player") && damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }

    IEnumerator DamagePlayer(Collider2D playerCollider)
    {
        CharacterHealth playerHealth = playerCollider.GetComponentInParent<CharacterHealth>();
        while(playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            yield return new WaitForSeconds(attackInterval);
        }
    }
}
