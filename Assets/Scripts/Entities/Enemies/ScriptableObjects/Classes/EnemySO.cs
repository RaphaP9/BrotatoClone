using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySO : EntitySO
{
    [Header("Enemy Stats Settings")]
    [Range(0,100)] public int goldDrop;
    [Space]
    [Range(0.5f, 5f)] public float spawnDuration;
    [Range(1f, 10f)] public float cleanupTime;
    [Space]
    [Range(0f, 1f)] public float critChance;
    [Range(0.5f, 3f)] public float critDamageMultiplier;
    [Space]
    public Transform enemyPrefab;
}
