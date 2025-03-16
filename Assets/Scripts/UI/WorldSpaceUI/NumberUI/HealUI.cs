using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealUI : NumberFeedbackUI
{
    [Header("Heal Settings")]
    [SerializeField, ColorUsage(true,true)] private Color healColor;

    private const string HEAL_EXTRA_CHARACTER = "+";

    public void SetHealUI(int heal)
    {
        string healText = HEAL_EXTRA_CHARACTER + heal;

        SetText(healText);
        SetTextColor(healColor);
    }
}
