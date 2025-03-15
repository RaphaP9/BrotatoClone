using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data.Common;

public abstract class MeleeWeaponHandler : AttackBasedWeaponHandler
{
    [Header("Settings")]
    [SerializeField] protected LayerMask enemyLayerMask;
    protected MeleeWeaponSO MeleeWeaponSO => weaponSO as MeleeWeaponSO;

    public static event EventHandler<OnMeleeWeaponAttackEventArgs> OnMeleeAttack;
    public event EventHandler<OnMeleeWeaponAttackEventArgs> OnThisMeleeAttack;

    public class OnMeleeWeaponAttackEventArgs : OnWeaponAttackEventArgs
    {
        public List<Transform> attackPoints;
    }

    protected void MeleeAttack(List<Transform> attackPoints)
    {
        bool isCrit = GeneralGameplayUtilities.EvaluateCritAttack(GetWeaponModifiedCritChance());

        int damage = GetWeaponModifiedRegularDamage();
        int bleedDamage = GetWeaponModifiedBleedDamage();
        float areaRadius = GetWeaponModifiedArea();

        List<Vector2> positions = GeneralUtilities.TransformPositionVector2List(attackPoints);

        if (isCrit)
        {
            damage = GeneralGameplayUtilities.CalculateCritDamage(damage, GetWeaponModifiedCritDamageMultiplier());
            bleedDamage = GeneralGameplayUtilities.CalculateCritDamage(bleedDamage, GetWeaponModifiedCritDamageMultiplier());
        }

        if (HasRegularDamage() && HasBleedDamage())
        {
            GeneralGameplayUtilities.DealRegularAndBleedDodgeableDamageInArea(damage, bleedDamage, MeleeWeaponSO.bleedDuration, MeleeWeaponSO.bleedTickTime, positions, areaRadius, isCrit, enemyLayerMask, MeleeWeaponSO);
            return;
        }

        if (HasRegularDamage())
        {
            GeneralGameplayUtilities.DealDodgeableRegularDamageInArea(damage, positions, areaRadius, isCrit, enemyLayerMask, MeleeWeaponSO);
            return;
        }

        if (HasBleedDamage())
        {
            GeneralGameplayUtilities.DealDodgeableBleedDamageInArea(bleedDamage, MeleeWeaponSO.bleedDuration, MeleeWeaponSO.bleedTickTime, positions, areaRadius, isCrit, enemyLayerMask, MeleeWeaponSO);

            return;
        }

        OnMeleeAttack?.Invoke(this, new OnMeleeWeaponAttackEventArgs { id = MeleeWeaponSO.id, isCrit = isCrit, attackPoints = attackPoints });
        OnThisMeleeAttack?.Invoke(this, new OnMeleeWeaponAttackEventArgs { id = MeleeWeaponSO.id, isCrit = isCrit, attackPoints = attackPoints});
    }


    protected bool HasRegularDamage() => MeleeWeaponSO.regularDamage > 0;
    protected bool HasBleedDamage() => MeleeWeaponSO.bleedDamage > 0;

    protected float GetWeaponModifiedArea() => GeneralGameplayUtilities.GetWeaponModifiedArea(MeleeWeaponSO.attackArea, AreaMultiplierStatManager.Instance.AreaMultiplierStat);
}
