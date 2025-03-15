using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntitySO : ScriptableObject, IDamageDealer
{
    [Header("Entity Identifiers")]
    public int id;
    public string entityName;
    [TextArea(3, 10)] public string description;
    public Sprite sprite;
    [ColorUsage(true, true)] public Color damageColor;

    [Header("Entity Stats")]
    [Range(10, 100)] public int maxHealth;
    [Range(0f, 1f)] public float armorPercentage;
    [Range(0f, 1f)] public float dodgeChance;
    [Space]
    [Range(1f, 10f)] public float moveSpeed;
    [Space]
    [Range(0f, 1f)] public float lifeSteal;

    public string GetName() => entityName;
    public string GetDescription() => description;
    public Sprite GetSprite() => sprite;
    public Color GetDamageColor() => damageColor;

}
