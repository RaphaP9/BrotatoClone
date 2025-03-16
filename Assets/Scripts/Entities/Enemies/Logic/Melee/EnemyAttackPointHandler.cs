using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackPointHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private EnemyAttack enemyAttack;
    [SerializeField] private EnemySpawningHandler enemySpawningHandler;
    [SerializeField] private EnemyHealth enemyHealth;
    [Space]
    [SerializeField] private Transform attackPointsHolder;

    private void Update()
    {
        HandlePointDueToMovement();
    }

    private void HandlePointDueToMovement()
    {
        if (enemySpawningHandler.IsSpawning) return;
        if (!enemyHealth.IsAlive()) return;
        //if (enemyAttack.IsAttacking()) return;

        float aimAngle = CalculateAimAngle();
        SetAttackPointRotation(aimAngle);
    }

    private float CalculateAimAngle()
    {
        Vector2 normalizedAimVector = CalculateAimUnitaryVector();
        float aimAngle = GeneralUtilities.GetVector2AngleDegrees(normalizedAimVector);

        return aimAngle;
    }

    private Vector2 CalculateAimUnitaryVector()
    {
        Vector2 aimVector = GetPlayerPosition() - GeneralUtilities.TransformPositionVector2(transform);
        aimVector.Normalize();

        return aimVector;
    }

    private Vector2 GetPlayerPosition()
    {
        return GeneralUtilities.TransformPositionVector2(PlayerPositionHandler.Instance.Player);
    }

    private void SetAttackPointRotation(float angle)
    {
        attackPointsHolder.rotation = Quaternion.Euler(0, 0, angle);
    }
}
