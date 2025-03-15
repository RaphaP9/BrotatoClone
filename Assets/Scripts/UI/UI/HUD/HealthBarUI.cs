using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image healthBarImage;
    [Space]
    [SerializeField] private TextMeshProUGUI currentHealthText;
    [SerializeField] private TextMeshProUGUI maxHealthText;

    [Header("Settings")]
    [SerializeField,Range(0.01f,10f)] private float lerpSpeed;

    private float LERP_THRESHOLD = 0.01f;

    private int maxHealth;
    private int currentHealth;

    private float targetFill;
    private float currentFill;

    private void OnEnable()
    {
        PlayerHealth.OnPlayerMaxHealthSet += PlayerHealth_OnPlayerMaxHealthSet;
        PlayerHealth.OnPlayerCurrentHealthSet += PlayerHealth_OnPlayerCurrentHealthSet;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerMaxHealthSet -= PlayerHealth_OnPlayerMaxHealthSet;
        PlayerHealth.OnPlayerCurrentHealthSet -= PlayerHealth_OnPlayerCurrentHealthSet;
    }

    private void Start()
    {
        FillBarCompletetly();
    }

    private void FillBarCompletetly()
    {
        SetTargetFillAmountByFill(1f);
        SetCurrentFillByFill(targetFill);
        SetHealthBarFillAmountByFill(currentFill);
    }

    private void Update()
    {
        HandleHealthBarLerping();
    }

    private void HandleHealthBarLerping()
    {
        if (Mathf.Abs(currentFill - targetFill) <= LERP_THRESHOLD) return;

        SetCurrentFillByFill(Mathf.Lerp(currentFill, targetFill, lerpSpeed*Time.deltaTime));
        SetHealthBarFillAmountByFill(currentFill);
    }

    private void SetMaxHealth(int maxHealth) => this.maxHealth = maxHealth;
    private void SetCurrentHealth(int currentHealth) => this.currentHealth = currentHealth;

    private void SetTargetFillAmount(int currentHealth, int maxHealth) => targetFill = (float)currentHealth / maxHealth;
    private void SetTargetFillAmountByFill(float fill) => targetFill = fill;
    private void SetCurrentFillByFill(float fill) => currentFill = fill;

    private void SetHealthBarFillAmountByFill(float fillAmount)
    {
        GeneralUIUtilities.SetImageFillRatio(healthBarImage, fillAmount);
    }

    private void SetHealthBarTexts(int currentHealth, int maxHealth)
    {
        currentHealthText.text = currentHealth.ToString();
        maxHealthText.text = maxHealth.ToString();
    }

    private void PlayerHealth_OnPlayerMaxHealthSet(object sender, EntityHealth.OnEntityHealthEventArgs e)
    {
        SetMaxHealth(e.health);
        SetTargetFillAmount(currentHealth, maxHealth);
        SetHealthBarTexts(currentHealth, maxHealth);
    }

    private void PlayerHealth_OnPlayerCurrentHealthSet(object sender, EntityHealth.OnEntityHealthEventArgs e)
    {
        SetCurrentHealth(e.health);
        SetTargetFillAmount(currentHealth, maxHealth);
        SetHealthBarTexts(currentHealth, maxHealth);
    }
}
