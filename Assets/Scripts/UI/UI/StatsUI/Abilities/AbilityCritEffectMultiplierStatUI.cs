using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCritEffectMultiplierStatUI : StatUI
{
    private void OnEnable()
    {
        AbilityCritEffectMultiplierStatManager.OnAbilityCritEffectMultiplierStatInitialized += AbilityCritEffectMultiplierStatManager_OnAbilityCritEffectMultiplierStatInitialized;
        AbilityCritEffectMultiplierStatManager.OnAbilityCritEffectMultiplierStatUpdated += AbilityCritEffectMultiplierStatManager_OnAbilityCritEffectMultiplierStatUpdated;
    }

    private void OnDisable()
    {
        AbilityCritEffectMultiplierStatManager.OnAbilityCritEffectMultiplierStatInitialized -= AbilityCritEffectMultiplierStatManager_OnAbilityCritEffectMultiplierStatInitialized;
        AbilityCritEffectMultiplierStatManager.OnAbilityCritEffectMultiplierStatUpdated -= AbilityCritEffectMultiplierStatManager_OnAbilityCritEffectMultiplierStatUpdated;
    }

    protected override string ProcessCurrentValue(float currentValue) => GeneralGameplayUtilities.ProcessCurrentValueToPercentage(currentValue, 2);

    private void AbilityCritEffectMultiplierStatManager_OnAbilityCritEffectMultiplierStatInitialized(object sender, AbilityCritEffectMultiplierStatManager.OnAbilityCritEffectMultiplierStatEventArgs e)
    {
        UpdateUIByNewValue(e.abilityCritEffectMultiplierStat, playerIdentifier.PlayerSO.abilityCritEffectMultiplier);
    }

    private void AbilityCritEffectMultiplierStatManager_OnAbilityCritEffectMultiplierStatUpdated(object sender, AbilityCritEffectMultiplierStatManager.OnAbilityCritEffectMultiplierStatEventArgs e)
    {
        UpdateUIByNewValue(e.abilityCritEffectMultiplierStat, playerIdentifier.PlayerSO.abilityCritEffectMultiplier);
    }
}
