using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShopSettingsSO", menuName = "ScriptableObjects/Shop/ShopSettings")]

public class ShopSettingsSO : ScriptableObject
{
    [Header("Shop Size")]
    [Range(3, 7)]public int shopSize;

    [Header("Inventory Object Limits & Odds")]
    //Objects are not capped, they are treated as the default Inventory Object
    [Range(0, 3)] public int weaponsCap;
    [Range(0, 3)] public int abilitiesCap;
    [Range(0, 3)] public int elementsCap;
    [Space]
    [Range(1, 100)] public int objectsWeight;
    [Range(0, 100)] public int weaponsWeight;
    [Range(0, 100)] public int abilitiesWeight;
    [Range(0, 100)] public int elementsWeight;

    [Header("Rerolls")]
    [Range(1, 100)] public int rerollBaseCost;
    [Range(1, 10)] public int rerollCostIncreasePerReroll;

}
