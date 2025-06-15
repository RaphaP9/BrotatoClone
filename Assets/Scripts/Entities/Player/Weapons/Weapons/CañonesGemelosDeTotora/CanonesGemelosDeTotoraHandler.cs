using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonesGemelosDeTotoraHandler : RangedWeaponHandler
{
    [Header("Components")]
    [SerializeField] private Transform firePoint1;
    [SerializeField] private Transform firePoint2;
    [SerializeField] private Transform projectilePrefab;

    public event EventHandler OnCanonesGemelosDeTotoraAttack;

    protected override void Attack()
    {
        ShootProjectile(projectilePrefab, firePoint1, weaponAim.AimDirection);
        ShootProjectile(projectilePrefab, firePoint2, weaponAim.AimDirection);
        OnCanonesGemelosDeTotoraAttack?.Invoke(this, EventArgs.Empty);
    }

    private void OnDrawGizmos()
    {
        if (!debug) return;

        Gizmos.color = gizmosColor;
        Gizmos.DrawLine(firePoint1.position, firePoint1.position + GeneralUtilities.Vector2ToVector3(weaponAim.AimDirection) * RangedWeaponSO.projectileRange);
        Gizmos.DrawLine(firePoint2.position, firePoint2.position + GeneralUtilities.Vector2ToVector3(weaponAim.AimDirection) * RangedWeaponSO.projectileRange);
    }
}
