using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected List<StatModifier> statModifiers;

    [Header("Debug")]
    [SerializeField] protected bool debug;

    public List<StatModifier> StatModifiers => statModifiers;

    protected CharacterSO BaseStats => PlayerIdentifier.Instance.CharacterSO;


    protected virtual void OnEnable()
    {
        ObjectsInventoryManager.OnObjectAddedToInventory += ObjectsInventoryManager_OnObjectAddedToInventory;
        ObjectsInventoryManager.OnObjectRemovedFromInventory += ObjectsInventoryManager_OnObjectRemovedFromInventory;

        WeaponsInventoryManager.OnWeaponAddedToInventory += WeaponsInventoryManager_OnWeaponAddedToInventory;
        WeaponsInventoryManager.OnWeaponRemovedFromInventory += WeaponsInventoryManager_OnWeaponRemovedFromInventory;

        AbilitiesInventoryManager.OnAbilityAddedToInventory += AbilitiesInventoryManager_OnAbilityAddedToInventory;
        AbilitiesInventoryManager.OnAbilityRemovedFromInventory += AbilitiesInventoryManager_OnAbilityRemovedFromInventory;

        ElementsInventoryManager.OnElementAddedToInventory += ElementsInventoryManager_OnElementAddedToInventory;
        ElementsInventoryManager.OnElementRemovedFromInventory += ElementsInventoryManager_OnElementRemovedFromInventory;
    }

    protected virtual void OnDisable()
    {
        ObjectsInventoryManager.OnObjectAddedToInventory -= ObjectsInventoryManager_OnObjectAddedToInventory;
        ObjectsInventoryManager.OnObjectRemovedFromInventory -= ObjectsInventoryManager_OnObjectRemovedFromInventory;

        WeaponsInventoryManager.OnWeaponAddedToInventory -= WeaponsInventoryManager_OnWeaponAddedToInventory;
        WeaponsInventoryManager.OnWeaponRemovedFromInventory -= WeaponsInventoryManager_OnWeaponRemovedFromInventory;

        AbilitiesInventoryManager.OnAbilityAddedToInventory -= AbilitiesInventoryManager_OnAbilityAddedToInventory;
        AbilitiesInventoryManager.OnAbilityRemovedFromInventory -= AbilitiesInventoryManager_OnAbilityRemovedFromInventory;

        ElementsInventoryManager.OnElementAddedToInventory -= ElementsInventoryManager_OnElementAddedToInventory;
        ElementsInventoryManager.OnElementRemovedFromInventory -= ElementsInventoryManager_OnElementRemovedFromInventory;
    }

    protected virtual void Awake()
    {
        SetSingleton();
    }

    protected virtual void Start()
    {
        InitializeStat();
    }

    protected abstract void SetSingleton();
    protected abstract void InitializeStat();
    protected abstract void UpdateStat();

    protected float CalculateStatValue(float baseValue, float minValue, float maxValue)
    {
        float newValue = baseValue;

        foreach(StatModifier statModifier in statModifiers)
        {
            switch (statModifier.statModificationType)
            {
                case StatModificationType.RawValue:
                default:
                    newValue = ModifyStatByRawValue(newValue,statModifier.value,minValue,maxValue);
                    break;
                case StatModificationType.Percentage:
                    newValue = ModifyStatByPercentageValue(newValue, statModifier.value, minValue, maxValue);
                    break;
            }
        }

        return newValue;
    }

    protected float ModifyStatByRawValue(float baseStatValue, float statModifierRawValue, float minValue, float maxValue)
    {
        baseStatValue += statModifierRawValue;
        baseStatValue = ClampStatToValues(baseStatValue, minValue, maxValue);

        return baseStatValue;
    }

    protected float ModifyStatByPercentageValue(float baseStatValue, float statModifierPercentageValue, float minValue, float maxValue)
    {
        baseStatValue *= 1 + statModifierPercentageValue;
        baseStatValue = ClampStatToValues(baseStatValue, minValue, maxValue);

        return baseStatValue;
    }

    protected float ClampStatToValues(float statValue, float minValue, float maxValue)
    {
        statValue = statValue > maxValue ? maxValue : statValue;
        statValue = statValue < minValue ? minValue : statValue;

        return statValue;
    }

    public bool HasStatModifiers() => statModifiers.Count > 0;
    public int GetStatModifiersQuantity() => statModifiers.Count;

    protected abstract StatType GetStatType();

    protected void AddStatModifiers(string originGUID, List<EmbeddedStat> embeddedStats)
    {
        if (originGUID == "")
        {
            if (debug) Debug.Log("GUID is empty. StatModifiers will not be added");
            return;
        }

        foreach (EmbeddedStat embeddedStat in embeddedStats)
        {
            AddStatModifier(originGUID, embeddedStat);
        }
    }

    protected void AddStatModifier(string originGUID, EmbeddedStat embeddedStat)
    {
        if (embeddedStat.statType != GetStatType()) return;

        if(embeddedStat == null)
        {
            if (debug) Debug.Log("EmbeddedStat is null. StatModifier will not be added");
            return;
        }

        StatModifier statModifier = new StatModifier { originGUID = originGUID, statType = embeddedStat.statType, statModificationType = embeddedStat.statModificationType, value = embeddedStat.value };

        statModifiers.Add(statModifier);

        UpdateStat();
    }

    protected void RemoveStatModifiersByGUID(string originGUID)
    {
        if (originGUID == "")
        {
            if (debug) Debug.Log("GUID is empty. StatModifiesr will not be removed");
            return;
        }

        statModifiers.RemoveAll(statModifier => statModifier.originGUID == originGUID);

        UpdateStat();
    }

    #region Subscriptions
    private void ObjectsInventoryManager_OnObjectAddedToInventory(object sender, ObjectsInventoryManager.OnObjectEventArgs e)
    {
        AddStatModifiers(e.@object.GUID, e.@object.objectSO.embeddedStats);
    }

    private void ObjectsInventoryManager_OnObjectRemovedFromInventory(object sender, ObjectsInventoryManager.OnObjectEventArgs e)
    {
        RemoveStatModifiersByGUID(e.@object.GUID);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void WeaponsInventoryManager_OnWeaponAddedToInventory(object sender, WeaponsInventoryManager.OnWeaponEventArgs e)
    {
        AddStatModifiers(e.weapon.GUID, e.weapon.weaponSO.embeddedStats);
    }

    private void WeaponsInventoryManager_OnWeaponRemovedFromInventory(object sender, WeaponsInventoryManager.OnWeaponEventArgs e)
    {
        RemoveStatModifiersByGUID(e.weapon.GUID);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void AbilitiesInventoryManager_OnAbilityAddedToInventory(object sender, AbilitiesInventoryManager.OnAbilityEventArgs e)
    {
        AddStatModifiers(e.ability.GUID, e.ability.abilitySO.embeddedStats);
    }

    private void AbilitiesInventoryManager_OnAbilityRemovedFromInventory(object sender, AbilitiesInventoryManager.OnAbilityEventArgs e)
    {
        RemoveStatModifiersByGUID(e.ability.GUID);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void ElementsInventoryManager_OnElementAddedToInventory(object sender, ElementsInventoryManager.OnElementEventArgs e)
    {
        AddStatModifiers(e.element.GUID, e.element.elementSO.embeddedStats);
    }

    private void ElementsInventoryManager_OnElementRemovedFromInventory(object sender, ElementsInventoryManager.OnElementEventArgs e)
    {
        RemoveStatModifiersByGUID(e.element.GUID);
    }

    #endregion
}
