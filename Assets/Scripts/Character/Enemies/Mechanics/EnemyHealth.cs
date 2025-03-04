using UnityEngine;

public class EnemyHealth : CharacterHealth
{
    [SerializeField] private EnemyCoinDrop coinDrop;

    protected override void Die()
    {
        base.Die();
        if (coinDrop != null)
            coinDrop.DropCoins(transform);
    }
}
