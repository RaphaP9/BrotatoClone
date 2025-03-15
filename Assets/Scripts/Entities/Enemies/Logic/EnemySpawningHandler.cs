using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawningHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemyIdentifier enemyIdentifier;

    [Header("Settings")]
    [SerializeField] private bool isSpawning;

    public bool IsSpawning => isSpawning;

    public static event EventHandler<OnEnemySpawnEventArgs> OnEnemySpawnStart;
    public static event EventHandler<OnEnemySpawnEventArgs> OnEnemySpawnComplete;

    public event EventHandler<OnEnemySpawnEventArgs> OnThisEnemySpawnStart;
    public event EventHandler<OnEnemySpawnEventArgs> OnThisEnemySpawnComplete;

    public class OnEnemySpawnEventArgs
    {
        public int id;
    }

    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        isSpawning = true;

        OnEnemySpawnStart?.Invoke(this, new OnEnemySpawnEventArgs { id = enemyIdentifier.EnemySO.id });
        OnThisEnemySpawnStart?.Invoke(this, new OnEnemySpawnEventArgs { id = enemyIdentifier.EnemySO.id });

        yield return new WaitForSeconds(enemyIdentifier.EnemySO.spawnDuration);

        OnEnemySpawnComplete?.Invoke(this, new OnEnemySpawnEventArgs { id = enemyIdentifier.EnemySO.id });
        OnThisEnemySpawnComplete?.Invoke(this, new OnEnemySpawnEventArgs { id = enemyIdentifier.EnemySO.id });

        isSpawning = false;
    } 
}

