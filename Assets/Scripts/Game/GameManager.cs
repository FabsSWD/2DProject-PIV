using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int coins = 0;
    public int maxHealth = 100;
    public int currentHealth = 100;
    public float moveSpeed = 5f;
    public int maxJumps = 2;
    public int attackDamage = 10;
    public float healingMultiplier = 1f;
    public SecondaryAbilityManager.SecondaryAbility ability = SecondaryAbilityManager.SecondaryAbility.None;

    void Awake()
    {
        if(Instance == null)
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
            GameManager.Instance.ability = SecondaryAbilityManager.SecondaryAbility.None;
        }
    }
}
