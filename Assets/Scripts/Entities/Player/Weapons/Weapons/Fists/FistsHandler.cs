using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistsHandler : MeleeWeaponHandler
{
    [Header("Fists Components")]
    [SerializeField] private Transform attackPoint;

    public static event EventHandler<OnWeaponAttackEventArgs> OnFistsAttack;

    protected override void Attack()
    {
        bool isCrit = GeneralGameplayUtilities.EvaluateCritAttack(GetWeaponModifiedCritChance());

        int damage = GetWeaponModifiedRegularDamage();
        if (isCrit) damage = GeneralGameplayUtilities.CalculateCritDamage(damage, GetWeaponModifiedCritDamageMultiplier());

        GeneralGameplayUtilities.DealRegularDamageInArea(damage, GeneralUtilities.TransformPositionVector2(attackPoint), GetWeaponModifiedArea(), isCrit, enemyLayerMask, weaponSO);

        OnFistsAttack?.Invoke(this, new OnWeaponAttackEventArgs { damage = damage, isCrit = isCrit});
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;

        Gizmos.DrawWireSphere(attackPoint.position, GetWeaponModifiedArea());
    }
}
