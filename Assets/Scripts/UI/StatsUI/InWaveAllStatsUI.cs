using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InWaveAllStatsUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private CanvasGroup canvasGroup;

    private void OnEnable()
    {
        InWaveStatsUIOpeningManager.OnStatsUIOpen += StatsUIOpeningManager_OnStatsUIOpen;
        InWaveStatsUIOpeningManager.OnStatsUIClose += StatsUIOpeningManager_OnStatsUIClose;

        InWaveStatsUIOpeningManager.OnStatsUIOpenInmediately += StatsUIOpeningManager_OnStatsUIOpenInmediately;
        InWaveStatsUIOpeningManager.OnStatsUICloseInmediately += StatsUIOpeningManager_OnStatsUICloseInmediately;
    }

    private void OnDisable()
    {
        InWaveStatsUIOpeningManager.OnStatsUIOpen -= StatsUIOpeningManager_OnStatsUIOpen;
        InWaveStatsUIOpeningManager.OnStatsUIClose -= StatsUIOpeningManager_OnStatsUIClose;

        InWaveStatsUIOpeningManager.OnStatsUIOpenInmediately -= StatsUIOpeningManager_OnStatsUIOpenInmediately;
        InWaveStatsUIOpeningManager.OnStatsUICloseInmediately -= StatsUIOpeningManager_OnStatsUICloseInmediately;
    }


    private void OpenStatsInmediately()
    {
        GeneralUIUtilities.SetCanvasGroupAlpha(canvasGroup, 1f);
    }

    private void CloseStatsInmediately()
    {
        GeneralUIUtilities.SetCanvasGroupAlpha(canvasGroup, 0f);
    }

    private void OpenStats()
    {
        GeneralUIUtilities.SetCanvasGroupAlpha(canvasGroup, 1f);
    }

    private void CloseStats()
    {
        GeneralUIUtilities.SetCanvasGroupAlpha(canvasGroup, 0f);
    }


    #region Subscriptions
    private void StatsUIOpeningManager_OnStatsUIOpen(object sender, System.EventArgs e)
    {
        OpenStats();
    }

    private void StatsUIOpeningManager_OnStatsUIClose(object sender, System.EventArgs e)
    {
        CloseStats();
    }

    private void StatsUIOpeningManager_OnStatsUIOpenInmediately(object sender, System.EventArgs e)
    {
        OpenStatsInmediately();
    }

    private void StatsUIOpeningManager_OnStatsUICloseInmediately(object sender, System.EventArgs e)
    {
        CloseStatsInmediately();
    }
    #endregion
}
