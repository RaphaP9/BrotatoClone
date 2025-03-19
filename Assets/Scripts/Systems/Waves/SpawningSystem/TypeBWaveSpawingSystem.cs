using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeBWaveSpawingSystem : WaveSpawningSystemManager
{
    //Using the weight system, increase the total weight of the wave to each enemyWave by a factor * totalWeight.
    //As every weight grows higher, chances of spawn of every enemy tend to be equal.
    //The goal to achieve is similar weights for the weakest enemy and the strongest enemy as the normalized elapsed time of the wave aproaches to 1

    [Header("TypeBWaveSpawning System Settings")]
    [SerializeField, Range(0f, 3f)] private float weightNormalizedIncreaseFactor; //When the wave normalized elapsed time is 1, each enemy weight has increased by totalWeight * weightNormalizedIncreaseFactor

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
}
