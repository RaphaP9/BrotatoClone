using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCleanup : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemyIdentifier enemyIdentifier;
    [SerializeField] private EnemyHealth enemyHealth;

    private void OnEnable()
    {
        enemyHealth.OnThisEnemyDeath += EnemyHealth_OnThisEnemyDeath;
    }

    private void OnDisable()
    {
        enemyHealth.OnThisEnemyDeath -= EnemyHealth_OnThisEnemyDeath;
    }

    private void CleanUpEnemy()
    {
        Destroy(gameObject, enemyIdentifier.EnemySO.cleanupTime);
    }

    private void EnemyHealth_OnThisEnemyDeath(object sender, System.EventArgs e)
    {
        CleanUpEnemy();
    }
}
