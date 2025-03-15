using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected EnemyIdentifier enemyIdentifier;
    [SerializeField] protected EnemyHealth enemyHealth;
    [SerializeField] protected EnemySpawningHandler spawningHandler;

    protected bool CanMove()
    {
        if (spawningHandler.IsSpawning) return false;
        if (!enemyHealth.IsAlive()) return false;

        return true;
    }
}
