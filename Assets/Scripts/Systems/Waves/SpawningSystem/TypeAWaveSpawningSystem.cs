using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeAWaveSpawningSystem : WaveSpawningSystemManager
{
    protected override void StartWave(WaveSO waveSO)
    {
        StartCoroutine(StartWaveCoroutine(waveSO));
    }

    private IEnumerator StartWaveCoroutine(WaveSO waveSO)
    {
        yield return null;
    }
}
