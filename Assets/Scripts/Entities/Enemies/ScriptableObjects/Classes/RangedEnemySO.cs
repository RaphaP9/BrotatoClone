using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRangedEnemySO", menuName = "ScriptableObjects/Entities/Enemies/RangedEnemy")]
public class RangedEnemySO : EnemySO, IProjectileSpawner 
{
    [Header("Ranged Enemy Settings")]
    [Range(3f, 20f)] public float tooCloseDistance;
    [Range(3f, 20f)] public float preferredDistance;
    [Range(3f, 20f)] public float tooFarDistance;
    [Space]
    [Range(0f, 3f)] public float aimingTime;
    [Range(0f, 3f)] public float shootingTime;
    [Range(0f, 3f)] public float postShootTime; 

    [Header("Projectile Settings")]
    [Range(0, 10)] public int projectileRegularDamage;
    [Space]
    [Range(0f, 10)] public int projectileBleedDamage;
    [Range(2f, 10f)] public float projectileBleedDuration;
    [Range(0.25f, 2f)] public float projectileBleedTickTime;
    [Space]
    [Range(3f, 20f)] public float projectileRange;
    [Range(5f, 30f)] public float projectileSpeed;
    [Range(0f, 20f)] public float dispersionAngle;
    [Space]
    public ProjectileDamageType projectileDamageType;
    [Range(0f, 3f)] public float projectileArea;
    [Space]
    public Transform projectilePrefab;

    public ProjectileDamageType GetProjectileDamageType() => projectileDamageType;
    public float GetProjectileArea() => projectileArea;
}
