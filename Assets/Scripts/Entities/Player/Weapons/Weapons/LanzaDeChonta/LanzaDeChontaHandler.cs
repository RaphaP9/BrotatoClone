using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LanzaDeChontaHandler : MeleeWeaponHandler
{
    [Header("Fists Components")]
    [SerializeField] private List<Transform> attackPoints;

    public event EventHandler OnLanzaDeChontaAttack;

    protected override void Attack()
    {
        MeleeAttack(attackPoints);
        OnLanzaDeChontaAttack?.Invoke(this, EventArgs.Empty);
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
