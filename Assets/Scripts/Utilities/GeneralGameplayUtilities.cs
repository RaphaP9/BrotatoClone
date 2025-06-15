using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GeneralGameplayUtilities
{
    public const float PERSPECTIVE_SCALE_X = 1f;
    public const float PERSPECTIVE_SCALE_Y = 1f;

    private const string PERCENTAGE_CHARACTER = "%";
    private const string PLUS_CHARACTER = "+";

    private const bool DEBUG = true;

    public static Vector2 ScaleVector2ToPerspective(Vector2 baseVector)
    {
        Vector2 scaledVector = new Vector2(baseVector.x * PERSPECTIVE_SCALE_X, baseVector.y *PERSPECTIVE_SCALE_Y);
        return scaledVector;
    }

    #region DamageTakenProcessing

    public static bool CheckDodgeByTransform(Transform transform)
    {
        EntityHealth entityHealth = GetEntityHealthComponentByTransform(transform);

        if(entityHealth == null)
        {
            if (DEBUG) Debug.Log("Transform does not contain an EntityHealth component. Dodge check will be ignored");
            return false;
        }

        if (entityHealth.TryDodge()) return true;
        return false;
    }

    public static bool CheckGhostedByTransform(Transform transform)
    {
        EntityHealth entityHealth = GetEntityHealthComponentByTransform(transform);

        if (entityHealth == null)
        {
            if (DEBUG) Debug.Log("Transform does not contain an EntityHealth component. Ghost check will be ignored");
            return false;
        }

        if (entityHealth.IsGhosted) return true;
        return false;
    }

    public static bool CheckIsAliveByTransform(Transform transform)
    {
        EntityHealth entityHealth = GetEntityHealthComponentByTransform(transform);

        if (entityHealth == null)
        {
            if (DEBUG) Debug.Log("Transform does not contain an EntityHealth component. IsAlive check will be ignored");
            return false;
        }

        return entityHealth.IsAlive();
    }

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

    #region StatProcessing
    public static int GetModifiedDamage(int damage, float attackDamageModificationPercentage)
    {
        float modifiedAttackDamage = damage * attackDamageModificationPercentage;
        int roundedModificatedDamage = Mathf.CeilToInt(modifiedAttackDamage);
        return roundedModificatedDamage;
    }

    public static float GetModifiedCritChance(float baseWeaponCritChance, float critChanceModificationValue)
    {
        float modifiedCritChande = baseWeaponCritChance + critChanceModificationValue;
        return modifiedCritChande;
    }

    public static float GetModifiedCritDamageMultiplier(float baseWeaponCritDamageMultiplier, float critDamageModificationValue)
    {
        float modifiedCritDamage = baseWeaponCritDamageMultiplier + critDamageModificationValue;
        return modifiedCritDamage;
    }

    public static float GetModifiedAttackSpeed(float baseWeaponAttackSpeed, float attackSpeedModificationPercentage)
    {
        float modifiedAttackSpeed = baseWeaponAttackSpeed * attackSpeedModificationPercentage;
        return modifiedAttackSpeed;
    }

    public static float GetWeaponModifiedArea(float baseWeaponArea, float areaModificationPercentage)
    {
        float modifiedArea = baseWeaponArea * areaModificationPercentage;
        return modifiedArea;
    }

    public static float GetWeaponModifiedRange(float baseWeaponRange, float rangeModificationPercentage)
    {
        float modifiedRange = baseWeaponRange * rangeModificationPercentage;
        return modifiedRange;
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
        int roundedCritDamage = Mathf.CeilToInt(critDamage);

        return roundedCritDamage;
    }
    #endregion

    #region DamageDealing

    public static void DealRegularAndBleedDodgeableDamageInArea(int regularDamage, int bleedDamage, float bleedDuration, float tickTime, List<Vector2> positions, float areaRadius, bool isCrit, LayerMask layermask, IDamageDealer damageSource)
    {
        List<Transform> detectedEnemyTransforms = DetectTransformsInMultipleRanges(positions, areaRadius, layermask);
        List<EntityHealth> entityHealthsInRange = GetEntityHealthComponentsByTransforms(detectedEnemyTransforms);

        foreach (EntityHealth entityHealth in entityHealthsInRange)
        {
            if (!entityHealth.IsAlive()) continue;
            if (entityHealth.TryDodge()) continue;

            entityHealth.TakeRegularDamage(regularDamage, isCrit, damageSource);
            entityHealth.Bleed(bleedDamage, bleedDuration, tickTime, isCrit, damageSource);
        }
    }

    public static void DealDodgeableRegularDamageInArea(int regularDamage, List<Vector2> positions, float areaRadius, bool isCrit, LayerMask layermask, IDamageDealer damageSource)
    {
        List<Transform> detectedEnemyTransforms = DetectTransformsInMultipleRanges(positions, areaRadius, layermask);
        List<EntityHealth> entityHealthsInRange = GetEntityHealthComponentsByTransforms(detectedEnemyTransforms);

        foreach (EntityHealth entityHealth in entityHealthsInRange)
        {
            if (!entityHealth.IsAlive()) continue;
            if (entityHealth.TryDodge()) continue;

            entityHealth.TakeRegularDamage(regularDamage, isCrit, damageSource);
        }
    }

    public static void DealDodgeableBleedDamageInArea(int bleedDamage, float bleedDuration, float tickTime, List<Vector2> positions, float areaRadius, bool isCrit, LayerMask layermask, IDamageDealer damageSource)
    {
        List<Transform> detectedEnemyTransforms = DetectTransformsInMultipleRanges(positions, areaRadius, layermask);
        List<EntityHealth> entityHealthsInRange = GetEntityHealthComponentsByTransforms(detectedEnemyTransforms);

        foreach (EntityHealth entityHealth in entityHealthsInRange)
        {
            if (!entityHealth.IsAlive()) continue;
            if (entityHealth.TryDodge()) continue;

            entityHealth.Bleed(bleedDamage, bleedDuration, tickTime, isCrit, damageSource);
        }
    }

    public static void DealRegularAndBleedDamageInArea(int regularDamage, int bleedDamage, float bleedDuration, float tickTime, List<Vector2> positions, float areaRadius, bool isCrit, LayerMask layermask, IDamageDealer damageSource)
    {
        List<Transform> detectedEnemyTransforms = DetectTransformsInMultipleRanges(positions, areaRadius, layermask);
        List<EntityHealth> entityHealthsInRange = GetEntityHealthComponentsByTransforms(detectedEnemyTransforms);

        foreach (EntityHealth entityHealth in entityHealthsInRange)
        {
            if (!entityHealth.IsAlive()) continue;

            entityHealth.TakeRegularDamage(regularDamage, isCrit, damageSource);
            entityHealth.Bleed(bleedDamage, bleedDuration, tickTime, isCrit, damageSource);
        }
    }

    public static void DealRegularDamageInArea(int regularDamage, List<Vector2> positions, float areaRadius, bool isCrit, LayerMask layermask, IDamageDealer damageSource)
    {
        List<Transform> detectedEnemyTransforms = DetectTransformsInMultipleRanges(positions, areaRadius, layermask);
        List<EntityHealth> entityHealthsInRange = GetEntityHealthComponentsByTransforms(detectedEnemyTransforms);

        foreach(EntityHealth entityHealth in entityHealthsInRange)
        {
            if (!entityHealth.IsAlive()) continue;

            entityHealth.TakeRegularDamage(regularDamage, isCrit, damageSource);
        }
    }

    public static void DealBleedDamageInArea(int bleedDamage, float bleedDuration, float tickTime, List<Vector2> positions, float areaRadius, bool isCrit, LayerMask layermask, IDamageDealer damageSource)
    {
        List<Transform> detectedEnemyTransforms = DetectTransformsInMultipleRanges(positions, areaRadius, layermask);
        List<EntityHealth> entityHealthsInRange = GetEntityHealthComponentsByTransforms(detectedEnemyTransforms);

        foreach (EntityHealth entityHealth in entityHealthsInRange)
        {
            if (!entityHealth.IsAlive()) continue;

            entityHealth.Bleed(bleedDamage, bleedDuration, tickTime, isCrit, damageSource);
        }
    }

    public static void DealRegularDamageToTransform(int damage, bool isCrit, Transform transform, IDamageDealer damageSource)
    {
        EntityHealth entityHealth = GetEntityHealthComponentByTransform(transform);

        if (entityHealth == null)
        {
            if (DEBUG) Debug.Log("Transform does not contain a EntityHealth component. Damage will be ignored.");
            return;
        }

        entityHealth.TakeRegularDamage(damage, isCrit, damageSource);  
    }

    public static void DealBleedDamageToTransform(int damage, float bleedDuration, float tickTime, bool isCrit, Transform transform, IDamageDealer damageSource)
    {
        EntityHealth entityHealth = GetEntityHealthComponentByTransform(transform);

        if (entityHealth == null)
        {
            if (DEBUG) Debug.Log("Transform does not contain a EntityHealth component. Damage will be ignored.");
            return;
        }

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
            if (DEBUG) Debug.Log("Transform does not contain an EntityHealthComponent.");
            return null;
        }

        return entityHealth;
    }
    #endregion

    #region Projectiles
    public static Vector2 DeviateShootDirection(Vector2 shootDirection, float dispersionAngle)
    {
        float randomAngle = Random.Range(-dispersionAngle, dispersionAngle);

        Vector2 deviatedDirection = GeneralUtilities.RotateVector2ByAngleDegrees(shootDirection, randomAngle);
        deviatedDirection.Normalize();

        return deviatedDirection;
    }
    #endregion

    #region StatUIProcessing
    public static string ProcessCurrentValueToSimpleInt(float currentValue)
    {
        int intValue = Mathf.RoundToInt(currentValue);
        string stringValue = intValue.ToString();
        return stringValue;
    }

    public static string ProcessCurrentValueToSimpleFloat(float currentValue, int decimalPlaces)
    {
        float floatValue = GeneralUtilities.RoundToNDecimalPlaces(currentValue, decimalPlaces);
        string stringValue = floatValue.ToString();
        return stringValue;
    }

    public static string ProcessCurrentValueToPercentage(float currentValue, int decimalPlaces)
    {
        float floatValue = GeneralUtilities.RoundToNDecimalPlaces(currentValue, decimalPlaces);
        string stringValue = TransformToPercentage(floatValue);
        return stringValue;
    }

    public static string ProcessCurrentValueToExcessPercentage(float currentValue, int decimalPlaces)
    {
        float floatValue = GeneralUtilities.RoundToNDecimalPlaces(currentValue, decimalPlaces);
        floatValue = floatValue - 1;
        string stringValue = TransformToPercentage(floatValue);
        return stringValue;
    }

    public static string TransformToPercentage(float value)
    {
        float percentageValue = value * 100;
        string stringValue = percentageValue.ToString() + PERCENTAGE_CHARACTER;
        return stringValue;
    }

    public static string ProcessNumericStatValueToString(StatType statType, float value)
    {
        switch (statType)
        {
            case StatType.MaxHealth:
            case StatType.HealthRegen:
            case StatType.Dashes:
                return ProcessCurrentValueToSimpleInt(value);
            case StatType.MoveSpeed:
            default:
                return ProcessCurrentValueToSimpleFloat(value, 2);
            case StatType.ArmorPercentage:
            case StatType.DodgeChance:
            case StatType.Lifesteal:
            case StatType.AttackCritChance:
            case StatType.AttackCritDamageMultiplier:
                return ProcessCurrentValueToPercentage(value, 2);
            case StatType.AreaMultiplier:
            case StatType.AttackSpeedMultiplier:
            case StatType.AttackRangeMultiplier:
            case StatType.AttackDamageMultiplier:
                return ProcessCurrentValueToExcessPercentage(value, 2);
        }
    }



    public static string ProcessObjectStatValueToString(StatType statType, StatModificationType statModificationType, float value)
    {
        string processedString = "";

        switch (statModificationType)
        {
            case StatModificationType.RawValue:
            default:
                processedString = ProcessObjectRawStatValueToString(statType, value);
                break;
            case StatModificationType.Percentage:
                processedString = ProcessObjectPercentageStatValueToString(statType, value);
                break;
        }

        if (value > 0f) processedString = PLUS_CHARACTER + processedString; //Add plus character to values over 0

        return processedString;
    }

    public static string ProcessObjectRawStatValueToString(StatType statType, float value)
    {
        switch (statType)
        {
            case StatType.MaxHealth:
            case StatType.HealthRegen:
            case StatType.Dashes:
                return ProcessCurrentValueToSimpleInt(value);
            case StatType.MoveSpeed:
            default:
                return ProcessCurrentValueToSimpleFloat(value, 2);
            case StatType.ArmorPercentage:
            case StatType.DodgeChance:
            case StatType.Lifesteal:
            case StatType.AttackCritChance:
            case StatType.AttackCritDamageMultiplier:
            case StatType.AreaMultiplier:
            case StatType.AttackSpeedMultiplier:
            case StatType.AttackRangeMultiplier:
            case StatType.AttackDamageMultiplier:
                return ProcessCurrentValueToPercentage(value, 2);

        }
        
    }

    public static string ProcessObjectPercentageStatValueToString(StatType statType, float value)
    {
        switch (statType)
        {
            case StatType.MaxHealth:
            case StatType.HealthRegen:
            case StatType.Dashes:
            case StatType.MoveSpeed:
            case StatType.ArmorPercentage:
            case StatType.DodgeChance:
            case StatType.Lifesteal:
            case StatType.AttackCritChance:
            case StatType.AttackCritDamageMultiplier:
            case StatType.AreaMultiplier:
            case StatType.AttackSpeedMultiplier:
            case StatType.AttackRangeMultiplier:
            case StatType.AttackDamageMultiplier:
            default:
                return ProcessCurrentValueToPercentage(value, 2);

        }

    }
    #endregion
}