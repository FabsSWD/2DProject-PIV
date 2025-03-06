using UnityEngine;

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
    public SecondaryAbility ability = SecondaryAbility.None;

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
}
