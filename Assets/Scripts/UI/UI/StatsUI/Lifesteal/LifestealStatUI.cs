using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifestealStatUI : StatUI
{
    private void OnEnable()
    {
        LifestealStatManager.OnLifestealStatInitialized += LifestealStatManager_OnLifestealStatInitialized;
        LifestealStatManager.OnLifestealStatUpdated += LifestealStatManager_OnLifestealStatUpdated;
    }
    private void OnDisable()
    {
        LifestealStatManager.OnLifestealStatInitialized -= LifestealStatManager_OnLifestealStatInitialized;
        LifestealStatManager.OnLifestealStatUpdated -= LifestealStatManager_OnLifestealStatUpdated;
    }

    protected override string ProcessCurrentValue(float currentValue) => GeneralGameplayUtilities.ProcessCurrentValueToPercentage(currentValue, 2);


    private void LifestealStatManager_OnLifestealStatInitialized(object sender, LifestealStatManager.OnLifestealStatEventArgs e)
    {
        UpdateUIByNewValue(e.lifestealStat, playerIdentifier.CharacterSO.lifeSteal);
    }

    private void LifestealStatManager_OnLifestealStatUpdated(object sender, LifestealStatManager.OnLifestealStatEventArgs e)
    {
        UpdateUIByNewValue(e.lifestealStat, playerIdentifier.CharacterSO.lifeSteal);
    }
}
