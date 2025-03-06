using UnityEngine;
using UnityEngine.UI;

public class EnemySystem : MonoBehaviour
{
    private WaveManager waveManager;
    public Text enemiesRemainingText;

    void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
        EnemyUpdate();
    }

    public void EnemyUpdate()
    {
        int wavesLeft = waveManager.totalWaves - waveManager.currentWave;
        if (enemiesRemainingText != null)
            enemiesRemainingText.text = waveManager.enemiesRemaining.ToString() + " || " +  wavesLeft.ToString();
    }
}
