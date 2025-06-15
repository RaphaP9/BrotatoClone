using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerbatanaDeCazadorHandler : RangedWeaponHandler
{
    [Header("Components")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform projectilePrefab;

    public event EventHandler OnCerbatanaDeCazadorAttack;

    protected override void Attack()
    {
        ShootProjectile(projectilePrefab, firePoint, weaponAim.AimDirection);
        OnCerbatanaDeCazadorAttack?.Invoke(this, EventArgs.Empty);
    }

    private void OnDrawGizmos()
    {
        if (!debug) return;

        Gizmos.color = gizmosColor;
        Gizmos.DrawLine(firePoint.position, firePoint.position + GeneralUtilities.Vector2ToVector3(weaponAim.AimDirection) * RangedWeaponSO.projectileRange);
    }
}
