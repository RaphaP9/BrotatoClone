using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKamikazeEnemySO", menuName = "ScriptableObjects/Entities/Enemies/KamikazeEnemy")]
public class KamikazeEnemySO : EnemySO
{
    [Header("Kamikaze Settings")]

    [Range(1, 10)] public int kamikazeRegularDamage;
    [Space]
    [Range(0f, 10)] public int kamikazeBleedDamage;
    [Range(2f, 10f)] public float kamikazeBleedDuration;
    [Range(0.25f, 2f)] public float kamikazeBleedTickTime;
    [Space]
    [Range(0f, 1f)] public float kamikazeExplosionTime;
    [Range(1f, 10f)] public float kamikazeDamageRange;
    [Range(1f, 10f)] public float kamikazeDetectionRange;
}
