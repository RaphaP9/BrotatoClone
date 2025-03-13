using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PostProcessingUIHandler : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] protected Button increaseIntensityButton;
    [SerializeField] protected Button decreaseIntensityButton;

    [Header("Components")]
    [SerializeField] List<OptionBarUI> optionBarUIs;

    protected PostProcessingManager postProcessingManager;

    protected const float INTENSITY_BUTTON_CHANGE = 0.1f;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        increaseIntensityButton.onClick.AddListener(IncreaseIntensityByButton);
        decreaseIntensityButton.onClick.AddListener(DecreaseIntensityByButton);

        IntializeOptionBarUIs();
    }

    private void Start()
    {
        InitializeUI();
    }

    protected void IntializeOptionBarUIs()
    {
        foreach (OptionBarUI optionBarUI in optionBarUIs)
        {
            optionBarUI.BackgroundButton.onClick.AddListener(() => postProcessingManager.ChangeIntensity(optionBarUI.BarValue));
        }
    }

    protected abstract void SetPostProcessingManager();

    protected void InitializeUI()
    {
        SetPostProcessingManager();

        if (!postProcessingManager) return;

        UpdateVisual();
    }

    private void IncreaseIntensityByButton()
    {
        float currentIntensity = postProcessingManager.GetNormalizedIntensity();
        float desiredIntensity = currentIntensity + INTENSITY_BUTTON_CHANGE;

        desiredIntensity = GeneralUtilities.RoundToNDecimalPlaces(desiredIntensity, 1);

        if (desiredIntensity > postProcessingManager.GetMaxNormalizedIntensity()) return;

        postProcessingManager.ChangeIntensity(desiredIntensity);
    }

    private void DecreaseIntensityByButton()
    {
        float currentIntensity = postProcessingManager.GetNormalizedIntensity();
        float desiredIntensity = currentIntensity - INTENSITY_BUTTON_CHANGE;

        desiredIntensity = GeneralUtilities.RoundToNDecimalPlaces(desiredIntensity, 1);

        if (desiredIntensity < postProcessingManager.GetMinNormalizedIntensity()) return;

        postProcessingManager.ChangeIntensity(desiredIntensity);
    }

    protected void UpdateVisual()
    {
        HideAllOptionBars();
        float currentValue = GeneralUtilities.RoundToNDecimalPlaces(postProcessingManager.GetNormalizedIntensity(), 1);

        foreach (OptionBarUI optionBarUI in optionBarUIs)
        {
            if (optionBarUI.BarValue <= currentValue) optionBarUI.ShowActiveIndicator();
            else optionBarUI.HideActiveIndicator();
        }
    }

    protected void HideAllOptionBars()
    {
        foreach (OptionBarUI optionBarUI in optionBarUIs)
        {
            optionBarUI.HideActiveIndicator();
        }
    }
}
