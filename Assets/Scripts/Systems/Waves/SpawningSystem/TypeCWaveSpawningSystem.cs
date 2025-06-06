using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeCWaveSpawingSystem : WaveSpawningSystemManager
{
    [Header("TypeCWaveSpawning System Settings")]
    [SerializeField, Range(0f, 3f)] private float weightNormalizedIncreaseFactor; //When the wave normalized elapsed time is 1, each enemy weight has increased by totalWeight * weightNormalizedIncreaseFactor
    [SerializeField, Range(0.2f, 0.8f)] private float spawnTimeNormalizedReductionFactor; //When the normalized elapsed time is 1, enemies spawn every baseSpawnTime * spawnTimeNormalizedReductionFactor

    private const float MINIMUM_SPAWN_TIME = 1f; 
    //Safety Reasons - If the system has a spawnTimeNormalizedReductionFactor of 1 or very close to 1, enemies will spawn very very fast which will affect performance
    //Should not happen because Range is set to a maximum of 0.5 but just in case

    public override void StartWave(WaveSO waveSO)
    {
        base.StartWave(waveSO);
        StartCoroutine(StartWaveCoroutine(waveSO));
    }

    private IEnumerator StartWaveCoroutine(WaveSO waveSO)
    {
        float waveElapsedTimer = 0f;
        float enemySpawnTimer = Mathf.Infinity; //To spawn an enemy inmediately after wave start

        while(waveElapsedTimer < waveSO.duration)
        {
            waveElapsedTimer += Time.deltaTime;
            enemySpawnTimer += Time.deltaTime;

            float normalizedElapsedTime = Mathf.Clamp01(waveElapsedTimer / waveSO.duration);

            if (enemySpawnTimer > GetDinamicSpawnTime(waveSO, normalizedElapsedTime))
            {
                EnemySO enemyToSpawn = GetRandomDinamicEnemyByWeight(waveSO, normalizedElapsedTime);
                EnemySpawnerManager.Instance.SpawnEnemy(enemyToSpawn);

                enemySpawnTimer = 0f;
            }

            SetCurrentWaveElapsedTime(waveElapsedTimer);

            yield return null;
        }

        CompleteWave(waveSO);
    }

    protected EnemySO GetRandomDinamicEnemyByWeight(WaveSO waveSO, float normalizedElapsedWaveTime)
    {
        float dinamicNormalizedWeightIncrease = CalculateDinamicNormalizedWeightIncrease(normalizedElapsedWaveTime);

        int totalWeight = GetTotalWaveWeight(waveSO);
        int singularDinamicWaveEnemyWeightIncrease = Mathf.RoundToInt(totalWeight * dinamicNormalizedWeightIncrease); //Weight that every enemy will increase

        int totalDinamicWeight = totalWeight + singularDinamicWaveEnemyWeightIncrease * waveSO.waveEnemies.Count; //Increase the total weight by the previous quantity times the number of wave enemies

        int randomValue = Random.Range(0, totalDinamicWeight); //Calculate the random value based on the totalDinamicWeight

        int currentWeight = 0;

        foreach (WaveEnemy waveEnemy in waveSO.waveEnemies)
        {
            currentWeight += waveEnemy.weight + singularDinamicWaveEnemyWeightIncrease; //Add the singularDinamicWeight to the real weight of the enemy

            if (randomValue <= currentWeight) return waveEnemy.enemySO;
        }

        return waveSO.waveEnemies[0].enemySO;
    }

    private float CalculateDinamicNormalizedWeightIncrease(float normalizedElapsedWaveTime) => weightNormalizedIncreaseFactor * normalizedElapsedWaveTime;

    protected float GetDinamicSpawnTime(WaveSO waveSO, float normalizedElapsedWaveTime)
    {
        float dinamicNormalizedSpawnTimeReduction = normalizedElapsedWaveTime * spawnTimeNormalizedReductionFactor;

        float dinamicSpawnTime = waveSO.baseSpawnTime * (1 - dinamicNormalizedSpawnTimeReduction);

        dinamicSpawnTime = Mathf.Max(dinamicSpawnTime, MINIMUM_SPAWN_TIME); //Safety Reasons - Spawn time should not be less than MINIMUM_SPAWN_TIME

        return dinamicSpawnTime;
    }
}
