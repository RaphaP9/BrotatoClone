using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class MeleeWeaponHandler : AttackBasedWeaponHandler
{
    [Header("Settings")]
    [SerializeField] protected LayerMask enemyLayerMask;
    protected MeleeWeaponSO MeleeWeaponSO => weaponSO as MeleeWeaponSO;

    public static event EventHandler<OnWeaponAttackEventArgs> OnMeleeAttack;
    public event EventHandler<OnWeaponAttackEventArgs> OnThisMeleeAttack;

    protected void MeleeAttack(Transform attackPoint)
    {
        bool isCrit = GeneralGameplayUtilities.EvaluateCritAttack(GetWeaponModifiedCritChance());

        int damage = GetWeaponModifiedRegularDamage();
        int bleedDamage = GetWeaponModifiedBleedDamage();
        float areaRadius = GetWeaponModifiedArea();
        Vector2 position = GeneralUtilities.TransformPositionVector2(attackPoint);

        if (isCrit)
        {
            damage = GeneralGameplayUtilities.CalculateCritDamage(damage, GetWeaponModifiedCritDamageMultiplier());
            bleedDamage = GeneralGameplayUtilities.CalculateCritDamage(bleedDamage, GetWeaponModifiedCritDamageMultiplier());
        }

        if (HasRegularDamage() && HasBleedDamage())
        {
            GeneralGameplayUtilities.DealRegularAndBleedDodgeableDamageInArea(damage, bleedDamage, MeleeWeaponSO.bleedDuration, MeleeWeaponSO.bleedTickTime, position, areaRadius, isCrit, enemyLayerMask, MeleeWeaponSO);
            return;
        }

        if (HasRegularDamage())
        {
            GeneralGameplayUtilities.DealDodgeableRegularDamageInArea(damage, position, areaRadius, isCrit, enemyLayerMask, MeleeWeaponSO);
            return;
        }

        if (HasBleedDamage())
        {
            GeneralGameplayUtilities.DealDodgeableBleedDamageInArea(bleedDamage, MeleeWeaponSO.bleedDuration, MeleeWeaponSO.bleedTickTime, position, areaRadius, isCrit, enemyLayerMask, MeleeWeaponSO);

            return;
        }

        OnMeleeAttack?.Invoke(this, new OnWeaponAttackEventArgs { id = MeleeWeaponSO.id, attackPoint = attackPoint, isCrit = isCrit });
        OnThisMeleeAttack?.Invoke(this, new OnWeaponAttackEventArgs { id = MeleeWeaponSO.id, attackPoint = attackPoint, isCrit = isCrit });
    }


    protected bool HasBleedDamage() => MeleeWeaponSO.regularDamage > 0f;
    protected bool HasRegularDamage() => MeleeWeaponSO.bleedDamage > 0f;

    protected float GetWeaponModifiedArea() => GeneralGameplayUtilities.GetWeaponModifiedArea(MeleeWeaponSO.attackArea, AreaMultiplierStatManager.Instance.AreaMultiplierStat);
}
