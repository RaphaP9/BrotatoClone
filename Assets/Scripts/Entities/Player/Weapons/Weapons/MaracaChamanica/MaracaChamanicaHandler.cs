using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaracaChamanicaHandler : MeleeWeaponHandler
{
    [Header("Fists Components")]
    [SerializeField] private List<Transform> attackPoints;

    public event EventHandler OnMaracaChamanicaAttack;

    protected override void Attack()
    {
        MeleeAttack(attackPoints);
        OnMaracaChamanicaAttack?.Invoke(this, EventArgs.Empty);
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
