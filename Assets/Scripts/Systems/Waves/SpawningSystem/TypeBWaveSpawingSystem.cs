using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeBWaveSpawingSystem : WaveSpawningSystemManager
{
    //Using the weight system, increase 50% of the total weight of the wave to each enemyWave.
    //As every weight grows higher, chances of spawn of every enemy tend to be equal.
    //The goal to achieve is similar weights for the weakest enemy and the strongest enemy as the normalized elapsed time of the wave aproaches to 1

    private const float WEIGHT_NORMALIZED_INCREASE_PERCENT_25 = 0.2f;
    private const float WEIGHT_NORMALIZED_INCREASE_PERCENT_50 = 0.2f;
    private const float WEIGHT_NORMALIZED_INCREASE_PERCENT_75 = 0.2f;

    private const float PERCENT_25 = 0.25f;
    private const float PERCENT_50 = 0.50f;
    private const float PERCENT_75 = 0.75f;

    protected override void StartWave(WaveSO waveSO)
    {
        StartCoroutine(StartWaveCoroutine(waveSO));        
    }

    private IEnumerator StartWaveCoroutine(WaveSO waveSO)
    {
        yield return null;
    }

    protected EnemySO GetRandomDinamicEnemyByWeight(WaveSO waveSO, float normalizedElapsedWaveTime)
    {
        float dinamicNormalizedWeightIncrease = 0;

        if (normalizedElapsedWaveTime > PERCENT_25) dinamicNormalizedWeightIncrease += WEIGHT_NORMALIZED_INCREASE_PERCENT_25;
        if (normalizedElapsedWaveTime > PERCENT_50) dinamicNormalizedWeightIncrease += WEIGHT_NORMALIZED_INCREASE_PERCENT_50;
        if (normalizedElapsedWaveTime > PERCENT_75) dinamicNormalizedWeightIncrease += WEIGHT_NORMALIZED_INCREASE_PERCENT_75;

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
}
