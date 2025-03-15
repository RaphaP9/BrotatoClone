using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKamikazeMovement : EnemyMovement
{
    [Header("Kamikaze Components")]
    [SerializeField] private EnemyKamikaze enemyKamikaze;

    private void Update()
    {
        HandleMovementTowardsPlayer();
    }

    protected override bool CanMove()
    {
        if (enemyKamikaze.IsExploding) return false;
        if (spawningHandler.IsSpawning) return false;
        if (!enemyHealth.IsAlive()) return false;

        return true;
    }

    private void HandleMovementTowardsPlayer()
    {
        if (!CanMove())
        {
            StopMovement();
            return;
        }

        Vector2 movementDirectionVector = GetNormalizedDirectionToPlayer();
        MoveTowardsDirection(movementDirectionVector, enemyIdentifier.EnemySO.moveSpeed);
    }
}
