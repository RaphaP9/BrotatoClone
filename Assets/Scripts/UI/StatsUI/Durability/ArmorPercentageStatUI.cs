using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorPercentageStatUI : StatUI
{
    private void OnEnable()
    {
        ArmorPercentageStatManager.OnArmorPercentageStatInitialized += ArmorPercentageStatManager_OnArmorPercentageStatInitialized;
        ArmorPercentageStatManager.OnArmorPercentageStatUpdated += ArmorPercentageStatManager_OnArmorPercentageStatUpdated;
    }
    
    private void OnDisable()
    {
        ArmorPercentageStatManager.OnArmorPercentageStatInitialized -= ArmorPercentageStatManager_OnArmorPercentageStatInitialized;
        ArmorPercentageStatManager.OnArmorPercentageStatUpdated -= ArmorPercentageStatManager_OnArmorPercentageStatUpdated;
    }

    protected override StatType GetStatType() => StatType.ArmorPercentage;


    private void ArmorPercentageStatManager_OnArmorPercentageStatInitialized(object sender, ArmorPercentageStatManager.OnArmorPercentageStatEventArgs e)
    {
        UpdateUIByNewValue(e.armorPercentageStat, playerIdentifier.CharacterSO.armorPercentage);
    }

    private void ArmorPercentageStatManager_OnArmorPercentageStatUpdated(object sender, ArmorPercentageStatManager.OnArmorPercentageStatEventArgs e)
    {
        UpdateUIByNewValue(e.armorPercentageStat, playerIdentifier.CharacterSO.armorPercentage);
    }
}
