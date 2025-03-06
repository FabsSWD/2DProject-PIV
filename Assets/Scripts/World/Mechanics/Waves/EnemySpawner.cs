using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
    public GameObject enemyPrefab;
    public float spawnCooldown = 1f;
    private WaveManager waveManager;

    void Start() {
        waveManager = FindObjectOfType<WaveManager>();
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies() {
        while (true) {
            if (waveManager != null && waveManager.waveActive && waveManager.CanSpawnEnemy()) {
                Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                waveManager.OnEnemySpawned();
                yield return new WaitForSeconds(spawnCooldown);
            } else {
                yield return null;
            }
        }
    }
}
