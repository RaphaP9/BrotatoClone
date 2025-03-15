using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementTowardsPlayer : EnemyMovement
{
    private void Update()
    {
        HandleMovementTowardsPlayer();
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
