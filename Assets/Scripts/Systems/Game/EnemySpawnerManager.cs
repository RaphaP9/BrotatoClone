using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    public static EnemySpawnerManager Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<EnemySpawnPoint> enemySpawnPoints;

    [Header("Settings")]
    [SerializeField,Range(3f, 10f)] private float minDistanceToPlayer;
    [SerializeField,Range(10f, 20f)] private float maxDistanceToPlayer;

    [Header("Debug")]
    [SerializeField] private bool debug;
    [SerializeField] private EnemySO test;

    [System.Serializable]
    public class EnemySpawnPoint
    {
        public int id;
        public Transform spawnPointTransform;
    }

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
        EnemySpawnPoint chosenSpawnPoint = GetRandomValidSpawnPoint(enemySpawnPoints, minDistanceToPlayer, maxDistanceToPlayer);

        if(chosenSpawnPoint == null)
        {
            if (debug) Debug.Log("Chosen SpawnPoint is null. Spawn will be ignored");
            return;
        }

        SpawnEnemyAtPosition(enemySO, chosenSpawnPoint.spawnPointTransform.position);
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

    private EnemySpawnPoint GetRandomValidSpawnPoint(List<EnemySpawnPoint> enemySpawnPointsPool, float minDistance, float maxDistance)
    {
        List<EnemySpawnPoint> validSpawnPoints = FilterValidEnemySpawnPointsByMinMaxDistanceRange(enemySpawnPointsPool, minDistance, maxDistance);
        EnemySpawnPoint chosenSpawnPoint = ChooseRandomEnemySpawnPoint(validSpawnPoints);

        return chosenSpawnPoint;
    }

    private List<EnemySpawnPoint> FilterValidEnemySpawnPointsByMinMaxDistanceRange(List<EnemySpawnPoint> enemySpawnPointsPool, float minDistance, float maxDistance)
    {
        List<EnemySpawnPoint> validSpawnPoints = new List<EnemySpawnPoint>();

        foreach(EnemySpawnPoint enemySpawnPoint in enemySpawnPointsPool)
        {
            if (!EnemySpawnPointOnMinDistanceRange(enemySpawnPoint, minDistance)) continue;
            if (!EnemySpawnPointOnMaxDistanceRange(enemySpawnPoint, maxDistance)) continue;

            validSpawnPoints.Add(enemySpawnPoint);
        }

        return validSpawnPoints;
    }

    private List<EnemySpawnPoint> FilterValidEnemySpawnPointsByMinDistanceRange(List<EnemySpawnPoint> enemySpawnPointsPool, float minDistance)
    {
        List<EnemySpawnPoint> validSpawnPoints = new List<EnemySpawnPoint>();

        foreach (EnemySpawnPoint enemySpawnPoint in enemySpawnPointsPool)
        {
            if (!EnemySpawnPointOnMinDistanceRange(enemySpawnPoint, minDistance)) continue;

            validSpawnPoints.Add(enemySpawnPoint);
        }

        return validSpawnPoints;
    }

    private List<EnemySpawnPoint> ChooseValidEnemySpawnPointsByMaxDistanceRange(List<EnemySpawnPoint> enemySpawnPointsPool, float maxDistance)
    {
        List<EnemySpawnPoint> validSpawnPoints = new List<EnemySpawnPoint>();

        foreach (EnemySpawnPoint enemySpawnPoint in enemySpawnPointsPool)
        {
            if (!EnemySpawnPointOnMaxDistanceRange(enemySpawnPoint, maxDistance)) continue;

            validSpawnPoints.Add(enemySpawnPoint);
        }

        return validSpawnPoints;
    }

    private EnemySpawnPoint ChooseRandomEnemySpawnPoint(List<EnemySpawnPoint> enemySpawnPointsPool)
    {
        EnemySpawnPoint enemySpawnPoint = GeneralUtilities.ChooseRandomElementFromList(enemySpawnPointsPool);
        return enemySpawnPoint;
    }

    private bool EnemySpawnPointOnMinDistanceRange(EnemySpawnPoint enemySpawnPoint, float minDistance)
    {
        if (Vector2.Distance(GeneralUtilities.TransformPositionVector2(enemySpawnPoint.spawnPointTransform), GeneralUtilities.TransformPositionVector2(PlayerPositionHandler.Instance.Player)) > minDistance) return true;
        return false;
    }

    private bool EnemySpawnPointOnMaxDistanceRange(EnemySpawnPoint enemySpawnPoint, float maxDistance)
    {
        if (Vector2.Distance(GeneralUtilities.TransformPositionVector2(enemySpawnPoint.spawnPointTransform), GeneralUtilities.TransformPositionVector2(PlayerPositionHandler.Instance.Player)) < maxDistance) return true;
        return false;
    }

    #endregion
}
