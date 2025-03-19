using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWaveSO", menuName = "ScriptableObjects/Wave")]
public class WaveSO : ScriptableObject
{
    public int waveNumber;
    [Range(10,120), Tooltip("In Seconds")] public int duration;
    [Range(1f, 10f)] public float baseSpawnTime;
    public List<WaveEnemy> waveEnemies;
}
