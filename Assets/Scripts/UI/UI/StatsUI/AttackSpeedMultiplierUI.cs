using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedMultiplierUI : StatUI
{
    private void OnEnable()
    {
        AttackSpeedMultiplierStatManager.OnAttackSpeedMultiplierStatInitialized += AttackSpeedMultiplierStatManager_OnAttackSpeedMultiplierStatInitialized;
        AttackSpeedMultiplierStatManager.OnAttackSpeedMultiplierStatUpdated += AttackSpeedMultiplierStatManager_OnAttackSpeedMultiplierStatUpdated;
    }
   
    private void OnDisable()
    {
        AttackSpeedMultiplierStatManager.OnAttackSpeedMultiplierStatInitialized -= AttackSpeedMultiplierStatManager_OnAttackSpeedMultiplierStatInitialized;
        AttackSpeedMultiplierStatManager.OnAttackSpeedMultiplierStatUpdated -= AttackSpeedMultiplierStatManager_OnAttackSpeedMultiplierStatUpdated;
    }

    protected override string ProcessCurrentValue(float currentValue) => GeneralGameplayUtilities.ProcessCurrentValueToExcessPercentage(currentValue, 2);

    private void AttackSpeedMultiplierStatManager_OnAttackSpeedMultiplierStatInitialized(object sender, AttackSpeedMultiplierStatManager.OnAttackSpeedMultiplierStatEventArgs e)
    {
        UpdateUIByNewValue(e.attackSpeedMultiplierStat, playerIdentifier.PlayerSO.attackSpeedMultiplier);
    }

    private void AttackSpeedMultiplierStatManager_OnAttackSpeedMultiplierStatUpdated(object sender, AttackSpeedMultiplierStatManager.OnAttackSpeedMultiplierStatEventArgs e)
    {
        UpdateUIByNewValue(e.attackSpeedMultiplierStat, playerIdentifier.PlayerSO.attackSpeedMultiplier);
    }
}
