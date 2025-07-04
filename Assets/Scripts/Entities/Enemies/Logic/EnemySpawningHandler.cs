using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawningHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemyIdentifier enemyIdentifier;
    [SerializeField] private EnemyHealth enemyHealth;

    [Header("Settings")]
    [SerializeField] private bool isSpawning;

    public bool IsSpawning => isSpawning;

    public static event EventHandler<OnEnemySpawnEventArgs> OnEnemySpawnStart;
    public static event EventHandler<OnEnemySpawnEventArgs> OnEnemySpawnComplete;

    public event EventHandler<OnEnemySpawnEventArgs> OnThisEnemySpawnStart;
    public event EventHandler<OnEnemySpawnEventArgs> OnThisEnemySpawnComplete;

    public event EventHandler<OnEnemySpawnEventArgs> OnThisEnemySpawnAlmostComplete;

    private const float ALMOST_COMPLETE_SPAWN_TIME = 0.2f;

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

        float spawningTimer = 0f;

        while (spawningTimer < enemyIdentifier.EnemySO.spawnDuration)
        {
            spawningTimer += Time.deltaTime;
            yield return null;
        }

        OnThisEnemySpawnAlmostComplete?.Invoke(this, new OnEnemySpawnEventArgs { id = enemyIdentifier.EnemySO.id });

        spawningTimer = 0f;

        while (spawningTimer < ALMOST_COMPLETE_SPAWN_TIME)
        {
            spawningTimer += Time.deltaTime;
            yield return null;
        }

        OnEnemySpawnComplete?.Invoke(this, new OnEnemySpawnEventArgs { id = enemyIdentifier.EnemySO.id });
        OnThisEnemySpawnComplete?.Invoke(this, new OnEnemySpawnEventArgs { id = enemyIdentifier.EnemySO.id });

        isSpawning = false;
    }
}

