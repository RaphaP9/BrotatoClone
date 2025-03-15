using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class EntityHealthBarUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] protected Image healthBarImage;
    [SerializeField] protected CanvasGroup healthBarCanvasGroup;
    [Space]
    [SerializeField] protected TextMeshProUGUI currentHealthText;
    [SerializeField] protected TextMeshProUGUI maxHealthText;

    protected int maxHealth;
    protected int currentHealth;

    protected void SetMaxHealth(int maxHealth)=> this.maxHealth = maxHealth;
    protected void SetCurrentHealth(int currentHealth)=> this.currentHealth = currentHealth;

    protected void UpdateHealthBar(int currentHealth,int maxHealth)
    {
        SetHealthBarTexts(currentHealth, maxHealth);
        SetHealthBarFillAmount(currentHealth,maxHealth);
        EnableHealthBar();

        if (currentHealth == maxHealth) DisableHealthBar();
        if (currentHealth == 0) DisableHealthBar();
    }

    protected void SetHealthBarFillAmount(int currentHealth, int maxHealth)
    {
        float ratio = (float)currentHealth / maxHealth;
        GeneralUIUtilities.SetImageFillRatio(healthBarImage, ratio);
    }

    protected void SetHealthBarTexts(int currentHealth, int maxHealth)
    {
        currentHealthText.text = currentHealth.ToString();
        maxHealthText.text = maxHealth.ToString();
    }

    protected void EnableHealthBar()
    {
        GeneralUIUtilities.SetCanvasGroupAlpha(healthBarCanvasGroup, 1f);
    }
    protected void DisableHealthBar()
    {
        GeneralUIUtilities.SetCanvasGroupAlpha(healthBarCanvasGroup, 0f);
    }

}
