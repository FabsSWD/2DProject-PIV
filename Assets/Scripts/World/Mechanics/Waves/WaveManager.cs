using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour {
    [Header("Configuración de oleadas")]
    public int totalWaves = 5;
    public int initialEnemyCount = 3;
    public int enemyIncrementPerWave = 2;
    public float preparationTime = 10f;

    [Header("Estado de la oleada")]
    public int currentWave = 0;
    private int enemiesToSpawn;
    private int enemiesRemaining;

    public bool WaveActive {
        get { return enemiesRemaining > 0; }
    }

    void Start() {
        StartCoroutine(PrepareAndStartNextWave());
    }

    IEnumerator PrepareAndStartNextWave() {
        yield return new WaitForSeconds(preparationTime);

        currentWave++;
        enemiesToSpawn = initialEnemyCount + (currentWave - 1) * enemyIncrementPerWave;
        enemiesRemaining = enemiesToSpawn;

        Debug.Log("Oleada " + currentWave + " iniciada. Enemigos a spawnear: " + enemiesToSpawn);
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
        Debug.Log("Enemigo muerto. Restantes en la oleada: " + enemiesRemaining);

        if (enemiesRemaining <= 0) {
            Debug.Log("Oleada " + currentWave + " completada.");
            if (currentWave < totalWaves) {
                StartCoroutine(PrepareAndStartNextWave());
            } else {
                Debug.Log("¡Todas las oleadas completadas!");
            }
        }
    }
}
