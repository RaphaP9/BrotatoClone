using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRangedEnemySO", menuName = "ScriptableObjects/Entities/Enemies/RangedEnemy")]
public class RangedEnemySO : EnemySO, IProjectileSpawner 
{
    [Header("Ranged Settings")]
    [Range(3f, 20f)] public float maxShootDistance;
    [Range(3f, 20f)] public float preferredShootDistance;
    [Range(3f, 20f)] public float minShootDistance;
    [Space] 
    [Range(0, 10)] public int projectileRegularDamage;
    [Space]
    [Range(0f, 10)] public int projectileBleedDamage;
    [Range(2f, 10f)] public float projectileBleedDuration;
    [Range(0.25f, 2f)] public float projectileBleedTickTime;
    [Space]
    public ProjectileDamageType projectileDamageType;
    [Range(0f, 3f)] public float projectileArea;

    public ProjectileDamageType GetProjectileDamageType() => projectileDamageType;
    public float GetProjectileArea() => projectileArea;
}
