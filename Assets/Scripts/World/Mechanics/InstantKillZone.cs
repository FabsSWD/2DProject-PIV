using UnityEngine;

public class InstantKillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterHealth health = collision.GetComponent<CharacterHealth>();
        if (health == null)
        {
            health = collision.GetComponentInParent<CharacterHealth>();
        }

        if (health != null)
        {
            health.TakeDamage(health.maxHealth);
        }
    }
}
