using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    #region WeaponStatProcessing
    public static int GetWeaponModifiedDamage(int damage, float attackDamageModificationPercentage)
    {
        float modifiedAttackDamage = damage * attackDamageModificationPercentage;
        int roundedModificatedDamage = Mathf.CeilToInt(modifiedAttackDamage);
        return roundedModificatedDamage;
    }

    public static float GetWeaponModifiedCritChance(float baseWeaponCritChance, float critChanceModificationValue)
    {
        float modifiedCritChande = baseWeaponCritChance + critChanceModificationValue;
        return modifiedCritChande;
    }

    public static float GetWeaponModifiedCritDamageMultiplier(float baseWeaponCritDamageMultiplier, float critDamageModificationValue)
    {
        float modifiedCritDamage = baseWeaponCritDamageMultiplier + critDamageModificationValue;
        return modifiedCritDamage;
    }

    public static float GetWeaponModifiedAttackSpeed(float baseWeaponAttackSpeed, float attackSpeedModificationPercentage)
    {
        float modifiedAttackSpeed = baseWeaponAttackSpeed * attackSpeedModificationPercentage;
        return modifiedAttackSpeed;
    }

    public static float GetWeaponModifiedArea(float baseWeaponArea, float areaModificationPercentage)
    {
        float modifiedArea = baseWeaponArea * areaModificationPercentage;
        return modifiedArea;
    }
    #endregion

    #region DamageProcessing
    public static bool EvaluateCritAttack(float critChance)
    {
        float randomNumber = Random.Range(0f, 1f);

        if (critChance >= randomNumber) return true;
        return false;
    }

    public static int CalculateCritDamage(int baseDamage, float critDamagePercentage)
    {
        float critDamage = baseDamage * critDamagePercentage;
        int roundedCritDamage = Mathf.CeilToInt(critDamagePercentage);

        return roundedCritDamage;
    }
    #endregion

    #region DamageDealing

    public static void DealRegularDamageInArea(int damage, Vector2 position, float areaRadius, bool isCrit, LayerMask layermask, IDamageDealer damageSource)
    {
        List<Transform> detectedEnemyTransforms = DetectTransformsInRange(position, areaRadius, layermask);
        List<EntityHealth> entityHealthsInRange = GetEntityHealthComponentsByTransforms(detectedEnemyTransforms);

        foreach(EntityHealth entityHealth in entityHealthsInRange)
        {
            entityHealth.TakeRegularDamage(damage, isCrit, damageSource);
        }
    }

    public static void DealBleedDamageInArea(int damage, float bleedDuration, float tickTime, Vector2 position, float areaRadius, bool isCrit, LayerMask layermask, IDamageDealer damageSource)
    {
        List<Transform> detectedEnemyTransforms = DetectTransformsInRange(position, areaRadius, layermask);
        List<EntityHealth> entityHealthsInRange = GetEntityHealthComponentsByTransforms(detectedEnemyTransforms);

        foreach (EntityHealth entityHealth in entityHealthsInRange)
        {
            entityHealth.Bleed(damage, bleedDuration, tickTime, isCrit, damageSource);
        }
    }

    public static void DealRegularDamageToTransform(int damage, bool isCrit, Transform transform, IDamageDealer damageSource)
    {
        EntityHealth entityHealth = GetEntityHealthComponentByTransform(transform);

        if (entityHealth == null) return;

        entityHealth.TakeRegularDamage(damage, isCrit, damageSource);  
    }

    public static void DealBleedDamageToTransform(int damage, float bleedDuration, float tickTime, bool isCrit, Transform transform, IDamageDealer damageSource)
    {
        EntityHealth entityHealth = GetEntityHealthComponentByTransform(transform);

        if (entityHealth == null) return;

        entityHealth.Bleed(damage, bleedDuration, tickTime, isCrit, damageSource);
    }


    #endregion

    #region Detection

    public static List<Transform> DetectTransformsInMultipleRanges(List<Vector2> positions,float detectionRange,LayerMask layerMask)
    {
        HashSet<Transform> uniqueTransforms = new HashSet<Transform>();

        foreach(Vector2 position in positions)
        {
            List<Transform> detectedTransforms = DetectTransformsInRange(position, detectionRange, layerMask);
            
            foreach(Transform transform in detectedTransforms)
            {
                uniqueTransforms.Add(transform);
            }
        }
      
        List<Transform> uniqueTransformsList = uniqueTransforms.ToList();
        return uniqueTransformsList;
    }

    public static List<Transform> DetectTransformsInRange(Vector2 position, float detectionRange, LayerMask layerMask)
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

    #region Projectiles
    public static Vector2 DeviateShootDirection(Vector2 shootDirection, float dispersionAngle)
    {
        return shootDirection;
    }
    #endregion
}
