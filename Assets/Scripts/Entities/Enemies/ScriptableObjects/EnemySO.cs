using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySO : EntitySO
{
    [Header("Enemy Stats Settings")]
    [Range(0,5)] public int oreDrop;
    [Space]
    [Range(1, 10)] public int kamikazeDamage;
    [Range(1f, 10f)] public float kamikazeDamageRange;
    [Range(1f, 10f)] public float kamikazeDetectionRange;
    [Space]
    [Range(1f, 5f)] public float spawnDuration;
    [Range(1f, 10f)] public float cleanupTime;
}
