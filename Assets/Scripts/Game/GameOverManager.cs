using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TriggerGameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }

    IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(5f);
        ResetGameManager();
        SceneManager.LoadScene(0);
    }

    void ResetGameManager()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.coins = 0;
            GameManager.Instance.maxHealth = 100;
            GameManager.Instance.currentHealth = 100;
            GameManager.Instance.moveSpeed = 5f;
            GameManager.Instance.maxJumps = 2;
            GameManager.Instance.attackDamage = 10;
            GameManager.Instance.healingMultiplier = 1f;
            GameManager.Instance.ability = SecondaryAbility.None;
        }
    }
}
