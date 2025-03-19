using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeAWaveSpawingSystem : WaveSpawningSystemManager
{
    protected override void StartWave(WaveSO waveSO)
    {
        StartCoroutine(TestCoroutine(waveSO));        
    }

    private IEnumerator TestCoroutine(WaveSO waveSO)
    {
        yield return new WaitForSeconds(waveSO.duration/10);
        CompleteWave(waveSO);
    }
}
