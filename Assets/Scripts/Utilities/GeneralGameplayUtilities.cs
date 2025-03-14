using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class GeneralGameplayUtilities
{
    public const float PERSPECTIVE_SCALE_X = 1f;
    public const float PERSPECTIVE_SCALE_Y = 1f;

    private const bool DEBUG = true;

    public static Vector2 ScaleVector2ToPerspective(Vector2 baseVector)
    {
        Vector2 scaledVector = new Vector2(baseVector.x * PERSPECTIVE_SCALE_X, baseVector.y *PERSPECTIVE_SCALE_Y);
        return scaledVector;
    }

    #region DamageTakenProcessing
    public static bool EvaluateDodgeChance(float dodgeChance)
    {
        float randomNumber = Random.Range(0f, 1f);

        if (dodgeChance >= randomNumber) return true;
        return false;
    }

    public static int MitigateDamageByPercentage(int baseDamage, float armor)
    {
        float clampedArmor = GeneralUtilities.ClampNumber01(armor);
        float resultingDamage = baseDamage * (1- clampedArmor);

        int roundedDamage = Mathf.CeilToInt(resultingDamage);

        return roundedDamage;
    }

    #endregion

    #region AttackSpeedProcessing
    public static float GetModifiedAttackSpeed(float baseAttackSpeed, float attackSpeedModificationPercentage)
    {
        float modifiedAttackSpeed = baseAttackSpeed * attackSpeedModificationPercentage;
        return modifiedAttackSpeed;
    }

    #endregion

    #region DamageProcessing
    public static bool EvaluateCritAttack(float critChance)
    {
        float randomNumber = Random.Range(0f, 1f);

        if (critChance >= randomNumber) return true;
        return false;
    }
    public static (bool, int) ProcessAttackDamage(int baseDamage, float critChance, float critDamagePercentage)
    {
        bool shouldCrit = EvaluateCritAttack(critChance);
        int damage;

        if (shouldCrit) damage = CalculateCritDamage(baseDamage, critDamagePercentage);
        else damage = baseDamage;

        return (shouldCrit, damage);
  
    }

    public static int CalculateCritDamage(int baseDamage, float critDamagePercentage)
    {
        float critDamage = baseDamage * critDamagePercentage;
        int roundedCritDamage = Mathf.CeilToInt(critDamagePercentage);

        return roundedCritDamage;
    }
    #endregion

    #region DamageDealing

    public static void DealRegularDamageInArea(int damage, Vector2 position, float areaRadius, bool isCrit, LayerMask layermask)
    {
        List<Transform> detectedEnemyTransforms = DetectTransforms(position, areaRadius, layermask);
        List<EntityHealth> entityHealthsInRange = GetEntityHealthComponentsByTransforms(detectedEnemyTransforms);

        foreach(EntityHealth entityHealth in entityHealthsInRange)
        {
            entityHealth.TakeRegularDamage(damage, isCrit);
        }
    }

    public static void DealBleedDamageInArea(int damage, float bleedDuration, float tickTime, Vector2 position, float areaRadius, bool isCrit, LayerMask layermask)
    {
        List<Transform> detectedEnemyTransforms = DetectTransforms(position, areaRadius, layermask);
        List<EntityHealth> entityHealthsInRange = GetEntityHealthComponentsByTransforms(detectedEnemyTransforms);

        foreach (EntityHealth entityHealth in entityHealthsInRange)
        {
            entityHealth.Bleed(damage, bleedDuration, tickTime, isCrit);
        }
    }

    public static void DealRegularDamageToTransform(int damage, bool isCrit, Transform transform)
    {
        EntityHealth entityHealth = GetEntityHealthComponentByTransform(transform);

        if (entityHealth == null) return;

        entityHealth.TakeRegularDamage(damage,isCrit);  
    }

    public static void DealBleedDamageToTransform(int damage, float bleedDuration, float tickTime, bool isCrit, Transform transform)
    {
        EntityHealth entityHealth = GetEntityHealthComponentByTransform(transform);

        if (entityHealth == null) return;

        entityHealth.Bleed(damage, bleedDuration, tickTime, isCrit);
    }


    #endregion

    #region Detection
    public static List<Transform> DetectTransforms(Vector2 position, float detectionRange, LayerMask layerMask)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, detectionRange, layerMask);

        List<Transform> detectedTransforms = GeneralUtilities.GetTransformsByColliders(colliders);

        return detectedTransforms;
    }

    public static List<EntityHealth> GetEntityHealthComponentsByTransforms(List<Transform> transforms)
    {
        List<EntityHealth> entityHealths = new List<EntityHealth>();

        foreach (Transform transform in transforms)
        {
            EntityHealth entityHealth = GetEntityHealthComponentByTransform(transform);

            if(entityHealth != null)
            {
                entityHealths.Add(entityHealth);
            }
        }

        return entityHealths;
    }

    private static EntityHealth GetEntityHealthComponentByTransform(Transform transform)
    {
        EntityHealth entityHealth = transform.GetComponent<EntityHealth>();

        if (entityHealth == null)
        {
            if (DEBUG) Debug.Log("Transform does not contain an EntityHealthComponent");
            return null;
        }

        return entityHealth;
    }
    #endregion

}
