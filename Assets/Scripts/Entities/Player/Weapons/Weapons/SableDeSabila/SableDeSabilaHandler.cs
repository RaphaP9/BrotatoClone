using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SableDeSabilaHandler : MeleeWeaponHandler
{
    [Header("Components")]
    [SerializeField] private List<Transform> attackPoints;

    public event EventHandler OnSableDeSabilaAttack;

    protected override void Attack()
    {
        MeleeAttack(attackPoints);
        OnSableDeSabilaAttack?.Invoke(this, EventArgs.Empty);
    }

    private void OnDrawGizmos()
    {
        if (!debug) return;

        Gizmos.color = gizmosColor;

        foreach (Transform attackPoint in attackPoints)
        {
            Gizmos.DrawWireSphere(attackPoint.position, GetWeaponModifiedArea());
        }
    }
}