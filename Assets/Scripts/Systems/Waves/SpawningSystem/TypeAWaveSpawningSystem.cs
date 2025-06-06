using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeAWaveSpawningSystem : WaveSpawningSystemManager
{
    public override void StartWave(WaveSO waveSO)
    {
        base.StartWave(waveSO);
        StartCoroutine(StartWaveCoroutine(waveSO));
    }

    private IEnumerator StartWaveCoroutine(WaveSO waveSO)
    {
        float waveElapsedTimer = 0f;
        float enemySpawnTimer = Mathf.Infinity; //To spawn an enemy inmediately after wave start

        while (waveElapsedTimer < waveSO.duration)
        {
            waveElapsedTimer += Time.deltaTime;
            enemySpawnTimer += Time.deltaTime;

            float normalizedElapsedTime = Mathf.Clamp01(waveElapsedTimer / waveSO.duration);

            if (enemySpawnTimer > waveSO.baseSpawnTime)
            {
                EnemySO enemyToSpawn = GetRandomEnemyByWeight(waveSO);
                EnemySpawnerManager.Instance.SpawnEnemy(enemyToSpawn);

                enemySpawnTimer = 0f;
            }

            SetCurrentWaveElapsedTime(waveElapsedTimer);

            yield return null;
        }

        CompleteWave(waveSO);
    }
}
