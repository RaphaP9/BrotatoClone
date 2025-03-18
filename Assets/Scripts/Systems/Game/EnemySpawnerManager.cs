using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    public static EnemySpawnerManager Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<Transform> enemySpawnPoints;

    [Header("Settings")]
    [SerializeField,Range(3f, 10f)] private float minDistanceToPlayer;
    [SerializeField,Range(10f, 20f)] private float maxDistanceToPlayer;

    [Header("Debug")]
    [SerializeField] private bool debug;
    [SerializeField] private Color gizmosColor;
    [SerializeField] private EnemySO test;

    private void Awake()
    {
        SetSingleton();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SpawnEnemy(test);
        }
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one EnemySpawnerManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void SpawnEnemy(EnemySO enemySO)
    {
        Transform chosenSpawnPoint = GetRandomValidSpawnPoint(enemySpawnPoints, minDistanceToPlayer, maxDistanceToPlayer);

        if(chosenSpawnPoint == null)
        {
            if (debug) Debug.Log("Chosen SpawnPoint is null. Spawn will be ignored");
            return;
        }

        SpawnEnemyAtPosition(enemySO, chosenSpawnPoint.position);
    }

    private void SpawnEnemyAtPosition(EnemySO enemySO, Vector3 position)
    {
        if(enemySO.enemyPrefab == null)
        {
            if (debug) Debug.Log($"EnemySO with name {enemySO.entityName} does not contain an enemy prefab. Instantiation will be ignored.");
            return;
        }

        Transform spawnedEnemy = Instantiate(enemySO.enemyPrefab,position, Quaternion.identity);
    }

    #region SpawnPoint Filtering

    private Transform GetRandomValidSpawnPoint(List<Transform> enemySpawnPointsPool, float minDistance, float maxDistance)
    {
        List<Transform> validSpawnPoints = FilterValidEnemySpawnPointsByMinMaxDistanceRange(enemySpawnPointsPool, minDistance, maxDistance);
        Transform chosenSpawnPoint = ChooseRandomEnemySpawnPoint(validSpawnPoints);

        return chosenSpawnPoint;
    }

    private List<Transform> FilterValidEnemySpawnPointsByMinMaxDistanceRange(List<Transform> enemySpawnPointsPool, float minDistance, float maxDistance)
    {
        List<Transform> validSpawnPoints = new List<Transform>();

        foreach(Transform enemySpawnPoint in enemySpawnPointsPool)
        {
            if (!EnemySpawnPointOnMinDistanceRange(enemySpawnPoint, minDistance)) continue;
            if (!EnemySpawnPointOnMaxDistanceRange(enemySpawnPoint, maxDistance)) continue;

            validSpawnPoints.Add(enemySpawnPoint);
        }

        return validSpawnPoints;
    }

    private List<Transform> FilterValidEnemySpawnPointsByMinDistanceRange(List<Transform> enemySpawnPointsPool, float minDistance)
    {
        List<Transform> validSpawnPoints = new List<Transform>();

        foreach (Transform enemySpawnPoint in enemySpawnPointsPool)
        {
            if (!EnemySpawnPointOnMinDistanceRange(enemySpawnPoint, minDistance)) continue;

            validSpawnPoints.Add(enemySpawnPoint);
        }

        return validSpawnPoints;
    }

    private List<Transform> ChooseValidEnemySpawnPointsByMaxDistanceRange(List<Transform> enemySpawnPointsPool, float maxDistance)
    {
        List<Transform> validSpawnPoints = new List<Transform>();

        foreach (Transform enemySpawnPoint in enemySpawnPointsPool)
        {
            if (!EnemySpawnPointOnMaxDistanceRange(enemySpawnPoint, maxDistance)) continue;

            validSpawnPoints.Add(enemySpawnPoint);
        }

        return validSpawnPoints;
    }

    private Transform ChooseRandomEnemySpawnPoint(List<Transform> enemySpawnPointsPool)
    {
        Transform enemySpawnPoint = GeneralUtilities.ChooseRandomElementFromList(enemySpawnPointsPool);
        return enemySpawnPoint;
    }

    private bool EnemySpawnPointOnMinDistanceRange(Transform enemySpawnPoint, float minDistance)
    {
        if (Vector2.Distance(GeneralUtilities.TransformPositionVector2(enemySpawnPoint), GeneralUtilities.TransformPositionVector2(PlayerPositionHandler.Instance.Player)) > minDistance) return true;
        return false;
    }

    private bool EnemySpawnPointOnMaxDistanceRange(Transform enemySpawnPoint, float maxDistance)
    {
        if (Vector2.Distance(GeneralUtilities.TransformPositionVector2(enemySpawnPoint), GeneralUtilities.TransformPositionVector2(PlayerPositionHandler.Instance.Player)) < maxDistance) return true;
        return false;
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;

        //Gizmos.DrawWireSphere(PlayerPositionHandler.Instance.Player.position, minDistanceToPlayer);
        //Gizmos.DrawWireSphere(PlayerPositionHandler.Instance.Player.position, maxDistanceToPlayer);
    }
}
