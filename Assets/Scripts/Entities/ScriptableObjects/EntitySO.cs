using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntitySO : ScriptableObject
{
    [Header("Entity Identifiers")]
    public int id;
    public string entityName;
    public Sprite sprite;

    [Header("Entity Stats")]
    [Range(10, 100)] public int maxHealth;
    [Range(0f, 1f)] public float armorPercentage;
    [Range(0f, 1f)] public float dodgeChance;
    [Space]
    [Range(1f, 10f)] public float moveSpeed;
    [Space]
    [Range(0.5f, 2f)] public float attackSpeed;
    [Range(1f, 4f)] public float attackRange;
    [Space]
    [Range(0f, 1f)] public float lifesteal;
}
