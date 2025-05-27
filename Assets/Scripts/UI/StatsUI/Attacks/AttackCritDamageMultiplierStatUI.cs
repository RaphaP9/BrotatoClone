using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCritDamageMultiplierStatUI : StatUI
{
    private void OnEnable()
    {
        AttackCritDamageMultiplierStatManager.OnAttackCritDamageMultiplierStatInitialized += AttackCritDamageMultiplierStatManager_OnAttackCritDamageMultiplierStatInitialized;
        AttackCritDamageMultiplierStatManager.OnAttackCritDamageMultiplierStatUpdated += AttackCritDamageMultiplierStatManager_OnAttackCritDamageMultiplierStatUpdated;
    }

    private void OnDisable()
    {
        AttackCritDamageMultiplierStatManager.OnAttackCritDamageMultiplierStatInitialized -= AttackCritDamageMultiplierStatManager_OnAttackCritDamageMultiplierStatInitialized;
        AttackCritDamageMultiplierStatManager.OnAttackCritDamageMultiplierStatUpdated -= AttackCritDamageMultiplierStatManager_OnAttackCritDamageMultiplierStatUpdated;
    }

    protected override StatType GetStatType() => StatType.AttackCritDamageMultiplier;

    private void AttackCritDamageMultiplierStatManager_OnAttackCritDamageMultiplierStatInitialized(object sender, AttackCritDamageMultiplierStatManager.OnAttackCritDamageMultiplierStatEventArgs e)
    {
        UpdateUIByNewValue(e.attackCritDamageMultiplierStat, playerIdentifier.CharacterSO.attackCritDamageMultiplier);
    }

    private void AttackCritDamageMultiplierStatManager_OnAttackCritDamageMultiplierStatUpdated(object sender, AttackCritDamageMultiplierStatManager.OnAttackCritDamageMultiplierStatEventArgs e)
    {
        UpdateUIByNewValue(e.attackCritDamageMultiplierStat, playerIdentifier.CharacterSO.attackCritDamageMultiplier);

    }
}
