using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EnemySO : ScriptableObject, IDamageDealer
{
    [Header("Enemy Identifiers")]
    public int id;
    public string enemyName;
    [TextArea(3, 10)] public string description;
    public Sprite sprite;

    [Header("Enemy Stats Settings")]
    [Range(10, 100)] public int maxHealth;
    [Range(0f, 1f)] public float armorPercentage;
    [Range(0f, 1f)] public float dodgeChance;
    [Space]
    [Range(0,5)] public int oreDrop;
    [Range(1f, 10f)] public float moveSpeed;
    [Space]
    [Range(1, 10)] public int collisionDamage;
    [Range(1f, 10f)] public float collisionDetectionRange;
    [Range(1f, 10f)] public float collisionDamageRange;
    [Space]
    [ColorUsage(true,true)] public Color damageColor;

    public string GetName() => enemyName;
    public Color GetDamageColor() => damageColor;
    public string GetDescription() => description;
    public Sprite GetSprite() => sprite;
}
