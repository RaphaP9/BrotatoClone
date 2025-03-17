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

    protected override string ProcessCurrentValue(float currentValue) => GeneralGameplayUtilities.ProcessCurrentValueToPercentage(currentValue, 2);

    private void AttackCritDamageMultiplierStatManager_OnAttackCritDamageMultiplierStatInitialized(object sender, AttackCritDamageMultiplierStatManager.OnAttackCritDamageMultiplierStatEventArgs e)
    {
        UpdateUIByNewValue(e.attackCritDamageMultiplierStat, playerIdentifier.PlayerSO.attackCritDamageMultiplier);
    }

    private void AttackCritDamageMultiplierStatManager_OnAttackCritDamageMultiplierStatUpdated(object sender, AttackCritDamageMultiplierStatManager.OnAttackCritDamageMultiplierStatEventArgs e)
    {
        UpdateUIByNewValue(e.attackCritDamageMultiplierStat, playerIdentifier.PlayerSO.attackCritDamageMultiplier);

    }
}
