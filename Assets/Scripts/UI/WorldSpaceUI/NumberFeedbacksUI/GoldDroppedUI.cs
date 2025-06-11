using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldDroppedUI : NumberFeedbackUI
{
    [Header("Gold Settings")]
    [SerializeField, ColorUsage(true, true)] private Color goldColor;

    private const string GOLD_EXTRA_CHARACTER = "+";

    public void SetGoldUI(int gold)
    {
        string healText = GOLD_EXTRA_CHARACTER + gold;

        SetText(healText);
        SetTextColor(goldColor);
    }
}
