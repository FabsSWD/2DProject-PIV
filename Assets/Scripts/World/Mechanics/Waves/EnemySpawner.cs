using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
    public GameObject enemyPrefab;
    public float spawnCooldown = 1f;

    private WaveManager waveManager;

    void Start() {
        waveManager = FindObjectOfType<WaveManager>();
        if (waveManager == null) {
            Debug.LogError("No se encontró un WaveManager en la escena.");
            return;
        }
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine() {
        while (true) {
            if (waveManager.WaveActive && waveManager.CanSpawnEnemy()) {
                Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                waveManager.OnEnemySpawned();
                yield return new WaitForSeconds(spawnCooldown);
            } else {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
