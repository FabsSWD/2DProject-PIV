using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int minHeal = 1;
    public int maxHeal = 10;
    public AudioClip healSound;
    private int healAmount;

    private HealthSystem healthSystem;

    void Start()
    {
        healthSystem = FindObjectOfType<HealthSystem>();
        healAmount = Random.Range(minHeal, maxHeal + 1);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterHealth health = other.GetComponentInParent<CharacterHealth>();
            if (health != null)
            {
                health.currentHealth = Mathf.Min(health.currentHealth + healAmount, health.maxHealth);
            }
            if (healSound != null)
                AudioSource.PlayClipAtPoint(healSound, transform.position);
            Destroy(gameObject);
            healthSystem.UpdateGraphics();
        }
    }
}
