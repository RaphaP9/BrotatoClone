using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PistolHandler : RangedWeaponHandler
{
    [Header("Pistol Components")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform projectilePrefab;

    public static event EventHandler<OnWeaponAttackEventArgs> OnPistolFire;

    protected override void Attack()
    {
        bool isCrit = GeneralGameplayUtilities.EvaluateCritAttack(GetWeaponModifiedCritChance());

        int damage = GetWeaponModifiedRegularDamage();
        if (isCrit) damage = GeneralGameplayUtilities.CalculateCritDamage(damage, GetWeaponModifiedCritDamageMultiplier());

        Vector2 firePosition = GeneralUtilities.TransformPositionVector2(firePoint);

        Vector2 shootDirection = weaponAim.AimDirection;
        Vector2 processedShootDirection = GeneralGameplayUtilities.DeviateShootDirection(shootDirection, RangedWeaponSO.dispersionAngle);

        ShootProjectile(projectilePrefab, firePosition, processedShootDirection);

        OnPistolFire?.Invoke(this, new OnWeaponAttackEventArgs { damage = damage, isCrit = isCrit });
    }
}
