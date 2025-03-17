using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamageMultiplierStatUI : StatUI
{
    private void OnEnable()
    {
        AttackDamageMultiplierStatManager.OnAttackDamageMultiplierStatInitialized += AttackDamageMultiplierStatManager_OnAttackDamageMultiplierStatInitialized;
        AttackDamageMultiplierStatManager.OnAttackDamageMultiplierStatUpdated += AttackDamageMultiplierStatManager_OnAttackDamageMultiplierStatUpdated;
    }

    private void OnDisable()
    {
        AttackDamageMultiplierStatManager.OnAttackDamageMultiplierStatInitialized -= AttackDamageMultiplierStatManager_OnAttackDamageMultiplierStatInitialized;
        AttackDamageMultiplierStatManager.OnAttackDamageMultiplierStatUpdated -= AttackDamageMultiplierStatManager_OnAttackDamageMultiplierStatUpdated;
    }

    protected override string ProcessCurrentValue(float currentValue) => GeneralGameplayUtilities.ProcessCurrentValueToExcessPercentage(currentValue, 2);


    private void AttackDamageMultiplierStatManager_OnAttackDamageMultiplierStatInitialized(object sender, AttackDamageMultiplierStatManager.OnAttackDamageMultiplierStatEventArgs e)
    {
        UpdateUIByNewValue(e.attackDamageMultiplierStat, playerIdentifier.PlayerSO.attackDamageMultiplier);

    }

    private void AttackDamageMultiplierStatManager_OnAttackDamageMultiplierStatUpdated(object sender, AttackDamageMultiplierStatManager.OnAttackDamageMultiplierStatEventArgs e)
    {
        UpdateUIByNewValue(e.attackDamageMultiplierStat, playerIdentifier.PlayerSO.attackDamageMultiplier);

    }
}
