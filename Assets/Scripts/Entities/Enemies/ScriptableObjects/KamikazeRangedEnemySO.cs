using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeRangedEnemySO : KamikazeEnemySO
{
    [Header("Ranged Settings")]

    [Range(0, 10)] public int regularDamage;
    [Space]
    [Range(0f, 10)] public int bleedDamage;
    [Range(2f, 10f)] public float bleedDuration;
    [Range(0.25f, 2f)] public float bleedTickTime;

}
