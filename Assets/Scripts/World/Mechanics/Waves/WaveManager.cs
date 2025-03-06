using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour {
    public int totalWaves = 5;
    public int currentWave = 0;
    public int initialEnemyCount = 3;
    public int enemyIncrementPerWave = 2;
    public float preparationTime = 10f;
    public int enemiesToSpawn;
    public int enemiesRemaining;
    public bool waveActive = false;

    void Start() {
        StartWave();
    }

    public void StartWave() {
        currentWave++;
        enemiesToSpawn = initialEnemyCount + (currentWave - 1) * enemyIncrementPerWave;
        enemiesRemaining = enemiesToSpawn;
        waveActive = true;
    }

    public bool CanSpawnEnemy() {
        return waveActive && enemiesToSpawn > 0;
    }

    public void OnEnemySpawned() {
        if (enemiesToSpawn > 0)
            enemiesToSpawn--;
    }

    public void OnEnemyKilled() {
        if (enemiesRemaining > 0)
            enemiesRemaining--;
        if (enemiesRemaining <= 0) {
            waveActive = false;
            if (currentWave < totalWaves)
                StartCoroutine(PrepareNextWave());
        }
    }

    private IEnumerator PrepareNextWave() {
        yield return new WaitForSeconds(preparationTime);
        StartWave();
    }
}
