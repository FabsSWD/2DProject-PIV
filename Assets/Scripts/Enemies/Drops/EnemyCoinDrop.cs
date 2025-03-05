using UnityEngine;

public class EnemyCoinDrop : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Transform lootDrop;
    [SerializeField] private int minCoins = 1;
    [SerializeField] private int maxCoins = 3;
    [SerializeField] private float horizontalForceMin = 0.5f;
    [SerializeField] private float horizontalForceMax = 1f;
    [SerializeField] private float verticalForceMin = 2f;
    [SerializeField] private float verticalForceMax = 3f;

    public void DropCoins(Transform enemyTransform)
    {
        if (coinPrefab == null || lootDrop == null)
        {
            Debug.LogWarning("Coin prefab or loot drop transform is not assigned.");
            return;
        }

        int coinsToDrop = Random.Range(minCoins, maxCoins + 1);
        int direction = enemyTransform.localScale.x > 0 ? -1 : 1;

        for (int i = 0; i < coinsToDrop; i++)
        {
            GameObject coin = Instantiate(coinPrefab, lootDrop.position, Quaternion.identity);
            Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float forceX = direction * Random.Range(horizontalForceMin, horizontalForceMax);
                float forceY = Random.Range(verticalForceMin, verticalForceMax);
                rb.AddForce(new Vector2(forceX, forceY), ForceMode2D.Impulse);
            }
        }
    }
}
