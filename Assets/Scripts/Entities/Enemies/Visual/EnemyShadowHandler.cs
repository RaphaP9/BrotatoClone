using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShadowHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject shadowGameObject;
    [Space]
    [SerializeField] private EnemySpawningHandler enemySpawningHandler;
    [SerializeField] private EnemyHealth enemyHealth;

    private void OnEnable()
    {
        enemySpawningHandler.OnThisEnemySpawnStart += EnemySpawningHandler_OnThisEnemySpawnStart;
        enemySpawningHandler.OnThisEnemySpawnComplete += EnemySpawningHandler_OnThisEnemySpawnComplete;

        enemyHealth.OnThisEnemyDeath += EnemyHealth_OnThisEnemyDeath;
    }

    private void OnDisable()
    {
        enemySpawningHandler.OnThisEnemySpawnStart -= EnemySpawningHandler_OnThisEnemySpawnStart;
        enemySpawningHandler.OnThisEnemySpawnComplete -= EnemySpawningHandler_OnThisEnemySpawnComplete;

        enemyHealth.OnThisEnemyDeath -= EnemyHealth_OnThisEnemyDeath;
    }

    private void EnableShadow() => shadowGameObject.SetActive(true);
    private void DisableShadow() => shadowGameObject.SetActive(false);

    private void EnemySpawningHandler_OnThisEnemySpawnStart(object sender, EnemySpawningHandler.OnEnemySpawnEventArgs e)
    {
        DisableShadow();
    }

    private void EnemySpawningHandler_OnThisEnemySpawnComplete(object sender, EnemySpawningHandler.OnEnemySpawnEventArgs e)
    {
        if (!enemyHealth.IsAlive()) return;
        EnableShadow();
    }

    private void EnemyHealth_OnThisEnemyDeath(object sender, EnemyHealth.OnEnemyDeathEventArgs e)
    {
        DisableShadow();
    }
}
