using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveEnemy 
{
    public EnemySO enemySO;
    [Range(1, 100)] public int weight;
}
