using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyDetector : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask enemiesLayerMask;

    private List<Transform> DetectEnemiesInRange(float detectionRange)
    {
        Collider2D[] detectedEnemiesColliders = Physics2D.OverlapCircleAll(GeneralUtilities.TransformPositionVector2(transform), detectionRange, enemiesLayerMask);

        List<Transform> detectedEnemies = GeneralUtilities.GetTransformsByColliders(detectedEnemiesColliders);

        return detectedEnemies;
    }

    public Transform GetClosestEnemyFromDetectedEnemies(float detectionRange)
    {
        List<Transform> detectedEnemies = DetectEnemiesInRange(detectionRange);

        if (detectedEnemies.Count <= 0) return null;

        Transform closestEnemy = detectedEnemies[0];

        foreach(Transform enemy in detectedEnemies)
        {
            float closestDistance = Vector2.Distance(GeneralUtilities.TransformPositionVector2(transform), GeneralUtilities.TransformPositionVector2(closestEnemy));
            float currentDistance = Vector2.Distance(GeneralUtilities.TransformPositionVector2(transform), GeneralUtilities.TransformPositionVector2(enemy));

            if(currentDistance < closestDistance) closestEnemy = enemy;
        }

        return closestEnemy;
    }
}
