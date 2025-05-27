using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class StatUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] protected TextMeshProUGUI valueText;
    [SerializeField] protected PlayerIdentifier playerIdentifier;

    [Header("Settings")]
    [SerializeField] protected Color positiveColor;
    [SerializeField] protected Color neutralColor;
    [SerializeField] protected Color negativeColor;

    protected void UpdateUIByNewValue(float currentValue, float baseValue)
    {
        StatState statState = GetStatState(currentValue, baseValue);

        switch (statState)
        {
            case StatState.Positive:
                SetValueTextColor(positiveColor);
                break;
            case StatState.Neutral:
                SetValueTextColor(neutralColor);
                break;
            case StatState.Negative:
                SetValueTextColor(negativeColor);
                break;
        }

        string processedValueText = GeneralGameplayUtilities.ProcessNumericStatValueToString(GetStatType(), currentValue);
        SetValueText(processedValueText);
    }

    protected abstract StatType GetStatType();
    protected void SetValueText(string text) => valueText.text = text;
    protected void SetValueTextColor(Color color) => valueText.color = color;

    protected StatState GetStatState(float currentValue, float baseValue)
    {
        if(currentValue > baseValue) return StatState.Positive;
        if(currentValue < baseValue) return StatState.Negative;

        return StatState.Neutral;
    }
}
