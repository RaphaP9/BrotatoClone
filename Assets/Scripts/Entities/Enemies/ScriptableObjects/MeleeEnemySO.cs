using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeEnemySO", menuName = "ScriptableObjects/Entities/Enemies/MeleeEnemy")]
public class MeleeEnemySO : EnemySO
{
    [Header("Melee Settings")]
    [Range(3f, 20f)] public float maxAttackDistance;
    [Space]
    [Range(0, 10)] public int attackRegularDamage;
    [Space]
    [Range(0f, 10)] public int attackBleedDamage;
    [Range(2f, 10f)] public float attackBleedDuration;
    [Range(0.25f, 2f)] public float attackBleedTickTime;
    [Space]
    [Range(0f, 3f)] public float attackArea;
}
