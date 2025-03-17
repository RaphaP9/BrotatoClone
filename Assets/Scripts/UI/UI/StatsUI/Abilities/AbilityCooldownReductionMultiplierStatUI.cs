using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCooldownReductionMultiplierStatUI : StatUI
{
    private void OnEnable()
    {
        AbilityCooldownReductionMultiplierStatManager.OnAbilityCooldownReductionMultiplierStatInitialized += AbilityCooldownReductionMultiplierStatManager_OnAbilityCooldownReductionMultiplierStatInitialized;
        AbilityCooldownReductionMultiplierStatManager.OnAbilityCooldownReductionMultiplierStatUpdated += AbilityCooldownReductionMultiplierStatManager_OnAbilityCooldownReductionMultiplierStatUpdated;
    }

    private void OnDisable()
    {
        AbilityCooldownReductionMultiplierStatManager.OnAbilityCooldownReductionMultiplierStatInitialized -= AbilityCooldownReductionMultiplierStatManager_OnAbilityCooldownReductionMultiplierStatInitialized;
        AbilityCooldownReductionMultiplierStatManager.OnAbilityCooldownReductionMultiplierStatUpdated -= AbilityCooldownReductionMultiplierStatManager_OnAbilityCooldownReductionMultiplierStatUpdated;
    }
    protected override string ProcessCurrentValue(float currentValue) => GeneralGameplayUtilities.ProcessCurrentValueToPercentage(currentValue, 2);

    private void AbilityCooldownReductionMultiplierStatManager_OnAbilityCooldownReductionMultiplierStatUpdated(object sender, AbilityCooldownReductionMultiplierStatManager.OnAbilityCooldownReductionMultiplierStatEventArgs e)
    {
        UpdateUIByNewValue(e.abilityCooldownReductionMultiplierStat, playerIdentifier.PlayerSO.abilityCooldownReductionMultiplier);
    }

    private void AbilityCooldownReductionMultiplierStatManager_OnAbilityCooldownReductionMultiplierStatInitialized(object sender, AbilityCooldownReductionMultiplierStatManager.OnAbilityCooldownReductionMultiplierStatEventArgs e)
    {
        UpdateUIByNewValue(e.abilityCooldownReductionMultiplierStat, playerIdentifier.PlayerSO.abilityCooldownReductionMultiplier);
    }
}
