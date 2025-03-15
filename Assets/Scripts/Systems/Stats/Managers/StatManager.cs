using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected List<StatModifier> statModifiers;

    public List<StatModifier> StatModifiers => statModifiers;

    protected PlayerSO BaseStats => PlayerIdentifier.Instance.PlayerSO;

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
}
