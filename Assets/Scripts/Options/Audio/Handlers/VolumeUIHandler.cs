using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class VolumeUIHandler : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] protected Button increaseVolumeButton;
    [SerializeField] protected Button decreaseVolumeButton;

    [Header("Components")]
    [SerializeField] List<OptionBarUI> optionBarUIs;

    protected VolumeManager volumeManager;

    protected const float VOLUME_BUTTON_CHANGE = 0.1f;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        increaseVolumeButton.onClick.AddListener(IncreaseVolumeByButton);
        decreaseVolumeButton.onClick.AddListener(DecreaseVolumeByButton);

        IntializeOptionBarUIs();
    }

    private void Start()
    {
        InitializeUI();
    }

    protected void IntializeOptionBarUIs()
    {
        foreach(OptionBarUI optionBarUI in optionBarUIs)
        {
            optionBarUI.BackgroundButton.onClick.AddListener(() => volumeManager.ChangeVolume(optionBarUI.BarValue));
        }
    }

    protected abstract void SetVolumeManager();

    protected void InitializeUI()
    {
        SetVolumeManager();

        if (!volumeManager) return;

        UpdateVisual();
    }

    private void IncreaseVolumeByButton()
    {
        float currentVolume = volumeManager.GetLinearVolume();
        float desiredVolume = currentVolume + VOLUME_BUTTON_CHANGE;

        if (desiredVolume > volumeManager.GetMaxVolume()) return;

        desiredVolume = GeneralMethods.RoundToNDecimalPlaces(desiredVolume, 1);
        volumeManager.ChangeVolume(desiredVolume);
    }

    private void DecreaseVolumeByButton()
    {
        float currentVolume = volumeManager.GetLinearVolume();
        float desiredVolume = currentVolume - VOLUME_BUTTON_CHANGE;

        if (desiredVolume < 0f) return;

        desiredVolume = GeneralMethods.RoundToNDecimalPlaces(desiredVolume, 1);
        volumeManager.ChangeVolume(desiredVolume);
    }

    protected void UpdateVisual()
    {
        HideAllOptionBars();
        float currentValue = GeneralMethods.RoundToNDecimalPlaces(volumeManager.GetLinearVolume(),1);

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

