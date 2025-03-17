using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class DamageTakenUI : NumberFeedbackUI
{
    [Header("Damage Taken Settings")]
    [SerializeField,Range(1f,1.5f)] private float critScaleMultiplier;

    private const string CRIT_EXTRA_CHARACTER = "!";

    public void SetDamageUI(int damage, Color color, bool isCrit)
    {
        string damageText = damage.ToString();

        if (isCrit) damageText += CRIT_EXTRA_CHARACTER;

        SetText(damageText);
        SetTextColor(color);

        if (isCrit) AugmentTextScale(critScaleMultiplier);
    }
}
