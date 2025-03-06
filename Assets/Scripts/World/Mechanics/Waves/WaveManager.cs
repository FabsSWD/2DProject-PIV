using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour {
    [Header("Wave Config")]
    public int totalWaves = 5;
    public int initialEnemyCount = 3;
    public int enemyIncrementPerWave = 2;
    public float preparationTime = 10f;

    [Header("Wave Status")]
    public int currentWave = 0;
    private int enemiesToSpawn;
    public int enemiesRemaining;
    private EnemySystem enemySystem;

    public bool WaveActive {
        get { return enemiesRemaining > 0; }
    }

    void Start() {
        enemySystem = FindObjectOfType<EnemySystem>();
        StartCoroutine(PrepareAndStartNextWave());
    }

    IEnumerator PrepareAndStartNextWave() {
        yield return new WaitForSeconds(preparationTime);

        currentWave++;
        enemiesToSpawn = initialEnemyCount + (currentWave - 1) * enemyIncrementPerWave;
        enemiesRemaining = enemiesToSpawn;
        enemySystem.EnemyUpdate();

        Debug.Log("Wave " + currentWave + " started. Enemies this round: " + enemiesToSpawn);
    }

    public bool CanSpawnEnemy() {
        return enemiesToSpawn > 0;
    }

    public void OnEnemySpawned() {
        if (enemiesToSpawn > 0)
            enemiesToSpawn--;
    }

    public void OnEnemyKilled() {
        enemiesRemaining--;
        Debug.Log("Enemies Remaining: " + enemiesRemaining);
        enemySystem.EnemyUpdate();

        if (enemiesRemaining <= 0) {
            Debug.Log("Wave " + currentWave + " completed.");
            if (currentWave < totalWaves) {
                StartCoroutine(PrepareAndStartNextWave());
            } else {
                Debug.Log("All waves cleared!");
            }
        }
    }
}
