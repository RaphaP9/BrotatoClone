using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeAWaveSpawingSystem : WaveSpawningSystemManager
{
    protected override void StartWave(Wave wave)
    {
        StartCoroutine(TestCoroutine(wave));        
    }

    private IEnumerator TestCoroutine(Wave wave)
    {
        yield return new WaitForSeconds(5);
        CompleteWave(wave);
    }
}
