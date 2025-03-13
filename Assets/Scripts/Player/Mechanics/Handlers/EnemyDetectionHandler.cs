using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyDetectionHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField,Range(1f,10f)] private float detectionRange;
    [SerializeField] private LayerMask enemiesLayerMask;

    [Header("Enemy List")]
    [SerializeField] private List<Transform> enemies;

    public List<Transform> Enemies => enemies;

    private void Update()
    {
        HandleEnemyDetection();
    }

    private void HandleEnemyDetection()
    {
        Collider2D[] detectedEnemiesColliders = Physics2D.OverlapCircleAll(GeneralUtilities.TransformPositionVector2(transform), detectionRange, enemiesLayerMask);

        List<Transform> detectedEnemies = GeneralUtilities.GetTransformsByColliders(detectedEnemiesColliders);

        //Get New and Old Enemies using Linq
        List<Transform> newEnemiesDetected = detectedEnemies.Except(enemies).ToList();
        List<Transform> oldEnemiesDetected = enemies.Except(detectedEnemies).ToList();

        //Add New Enemies Retected
        foreach (Transform newEnemyDetected in newEnemiesDetected)
        {
            enemies.Add(newEnemyDetected);
        }

        //Remove Old Enemies Detected
        foreach (Transform oldEnemyDetected in oldEnemiesDetected)
        {
            enemies.Remove(oldEnemyDetected);
        }

        foreach(Transform enemy in enemies)
        {
            if (enemy == null) Debug.Log("Null Detected");
        }

        enemies.RemoveAll(t => t == null); //Remove null transforms 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }


}
