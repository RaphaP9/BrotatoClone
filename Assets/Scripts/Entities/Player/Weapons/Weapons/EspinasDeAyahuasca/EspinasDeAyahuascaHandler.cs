using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EspinasDeAyahuascaHandler : RangedWeaponHandler
{
    [Header("Components")]
    [SerializeField] private Transform firePoint1;
    [SerializeField] private Transform firePoint2;
    [SerializeField] private Transform firePoint3;
    [SerializeField] private Transform projectilePrefab;
    [SerializeField, Range(10f,30f)] private float deviationAngle;

    public event EventHandler OnEspinasDeAyahuascaAttack;

    protected override void Attack()
    {
        //3 attacks
        ShootProjectile(projectilePrefab, firePoint1, weaponAim.AimDirection);
        ShootProjectile(projectilePrefab, firePoint2, GeneralUtilities.RotateVector2ByAngleDegrees(weaponAim.AimDirection, deviationAngle));
        ShootProjectile(projectilePrefab, firePoint3, GeneralUtilities.RotateVector2ByAngleDegrees(weaponAim.AimDirection, -deviationAngle));

        OnEspinasDeAyahuascaAttack?.Invoke(this, EventArgs.Empty);
    }

    private void OnDrawGizmos()
    {
        if(!debug) return;  

        Gizmos.color = gizmosColor;

        Gizmos.DrawLine(firePoint1.position, firePoint1.position + GeneralUtilities.Vector2ToVector3(weaponAim.AimDirection) * RangedWeaponSO.projectileRange);
    }
}