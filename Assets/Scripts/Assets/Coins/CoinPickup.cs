using UnityEngine;
public class CoinPickup : MonoBehaviour
{
    public int coinValue = 1;
    public AudioClip coinSound;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerInventory playerInventory = collision.GetComponentInParent<PlayerInventory>();
            if(playerInventory != null)
                playerInventory.AddCoins(coinValue);
            if(coinSound != null)
                AudioSource.PlayClipAtPoint(coinSound, transform.position);
            Destroy(gameObject);
        }
    }
}
