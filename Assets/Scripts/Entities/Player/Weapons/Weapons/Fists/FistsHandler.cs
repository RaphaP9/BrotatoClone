using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistsHandler : MeleeWeaponHandler
{
    [Header("FistsComponents")]
    [SerializeField] private Transform attackPoint;

    public static event EventHandler OnFistsAttack;

    protected override void Attack()
    {
        bool isCrit = GeneralGameplayUtilities.EvaluateCritAttack(GetWeaponModifiedCritChance());

        int damage = GetWeaponModifiedRegularDamage();
        if (isCrit) damage = GeneralGameplayUtilities.CalculateCritDamage(damage, GetWeaponModifiedCritDamageMultiplier());

        GeneralGameplayUtilities.DealRegularDamageInArea(damage, GeneralUtilities.TransformPositionVector2(attackPoint), GetWeaponModifiedArea(), isCrit, enemyLayerMask);

        OnFistsAttack?.Invoke(this, new OnWeaponAttackEventArgs { damage = damage, isCrit = isCrit});
    }
}
