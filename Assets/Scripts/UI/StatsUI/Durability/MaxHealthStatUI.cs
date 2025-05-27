using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthStatUI : StatUI
{
    private void OnEnable()
    {
        MaxHealthStatManager.OnMaxHealthStatInitialized += MaxHealthStatManager_OnMaxHealthStatInitialized;
        MaxHealthStatManager.OnMaxHealthStatUpdated += MaxHealthStatManager_OnMaxHealthStatUpdated;
    }

    private void OnDisable()
    {
        MaxHealthStatManager.OnMaxHealthStatInitialized -= MaxHealthStatManager_OnMaxHealthStatInitialized;
        MaxHealthStatManager.OnMaxHealthStatUpdated -= MaxHealthStatManager_OnMaxHealthStatUpdated;
    }

    protected override StatType GetStatType() => StatType.MaxHealth;

    private void MaxHealthStatManager_OnMaxHealthStatInitialized(object sender, MaxHealthStatManager.OnMaxHealthStatEventArgs e)
    {
        UpdateUIByNewValue(e.maxHealthStat, playerIdentifier.CharacterSO.maxHealth);
    }

    private void MaxHealthStatManager_OnMaxHealthStatUpdated(object sender, MaxHealthStatManager.OnMaxHealthStatEventArgs e)
    {
        UpdateUIByNewValue(e.maxHealthStat, playerIdentifier.CharacterSO.maxHealth);
    }
}
