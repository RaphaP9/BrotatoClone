using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityEffectMultiplierStatUI : StatUI
{
    private void OnEnable()
    {
        AbilityEffectMultiplierStatManager.OnAbilityEffectMultiplierStatInitialized += AbilityEffectMultiplierStatManager_OnAbilityEffectMultiplierStatInitialized;
        AbilityEffectMultiplierStatManager.OnAbilityEffectMultiplierStatUpdated += AbilityEffectMultiplierStatManager_OnAbilityEffectMultiplierStatUpdated;
    }

    private void OnDisable()
    {
        AbilityEffectMultiplierStatManager.OnAbilityEffectMultiplierStatInitialized -= AbilityEffectMultiplierStatManager_OnAbilityEffectMultiplierStatInitialized;
        AbilityEffectMultiplierStatManager.OnAbilityEffectMultiplierStatUpdated -= AbilityEffectMultiplierStatManager_OnAbilityEffectMultiplierStatUpdated;
    }

    protected override string ProcessCurrentValue(float currentValue) => GeneralGameplayUtilities.ProcessCurrentValueToExcessPercentage(currentValue, 2);


    private void AbilityEffectMultiplierStatManager_OnAbilityEffectMultiplierStatInitialized(object sender, AbilityEffectMultiplierStatManager.OnAbilityEffectMultiplierStatEventArgs e)
    {
        UpdateUIByNewValue(e.abilityEffectMultiplierStat, playerIdentifier.PlayerSO.abilityEffectMultiplier);
    }

    private void AbilityEffectMultiplierStatManager_OnAbilityEffectMultiplierStatUpdated(object sender, AbilityEffectMultiplierStatManager.OnAbilityEffectMultiplierStatEventArgs e)
    {
        UpdateUIByNewValue(e.abilityEffectMultiplierStat, playerIdentifier.PlayerSO.abilityEffectMultiplier);
    }

}
