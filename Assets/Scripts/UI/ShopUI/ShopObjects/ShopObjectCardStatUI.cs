using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopObjectCardStatUI : MonoBehaviour
{
    [Header("Runtime Filled")]
    [SerializeField] private EmbeddedStat embededStat;

    [Header("Components")]
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    [Header("Settings")]
    [SerializeField] protected Color positiveColor;
    [SerializeField] protected Color neutralColor;
    [SerializeField] protected Color negativeColor;


    public void SetEmbeddedStat(EmbeddedStat embededStat)
    {
        this.embededStat = embededStat;

        SetStatValueText(embededStat);
        SetStatNameText(embededStat);
    }

    private void SetStatNameText(EmbeddedStat embededStat) => statNameText.text = GeneralUIUtilities.MapStatType(embededStat.statType);

    private void SetStatValueText(EmbeddedStat embededStat)
    {
        StatState statState = GetStatState(embededStat.value); 

        switch (statState)
        {
            case StatState.Positive:
                SetStatValueTextColor(positiveColor);
                break;
            case StatState.Neutral:
                SetStatValueTextColor(neutralColor);
                break;
            case StatState.Negative:
                SetStatValueTextColor(negativeColor);
                break;
        }

        string processedValueText = GeneralGameplayUtilities.ProcessObjectStatValueToString(embededStat.statType, embededStat.statModificationType, embededStat.value);

        SetStatValueText(processedValueText);
    }

    private void SetStatValueText(string text) => statValueText.text = text;
    protected void SetStatValueTextColor(Color color) => statValueText.color = color;
    
    
    protected StatState GetStatState(float currentValue) //Base Value is always 0
    {
        if (currentValue > 0f) return StatState.Positive;
        if (currentValue < 0f) return StatState.Negative;

        return StatState.Neutral;
    }
}
