using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI waveText;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private void OnEnable()
    {
        GeneralWavesManager.OnWaveStart += GeneralWavesManager_OnWaveStart;
        GeneralWavesManager.OnWaveCompleted += GeneralWavesManager_OnWaveCompleted;
    }

    private void OnDisable()
    {
        GeneralWavesManager.OnWaveStart -= GeneralWavesManager_OnWaveStart;
        GeneralWavesManager.OnWaveCompleted -= GeneralWavesManager_OnWaveCompleted;
    }

    private void SetWaveText(int waveNumber) => waveText.text = $"Oleada {waveNumber}";

    public void ShowUI()
    {
        animator.ResetTrigger(HIDE_TRIGGER);
        animator.SetTrigger(SHOW_TRIGGER);
    }

    public void HideUI()
    {
        animator.ResetTrigger(SHOW_TRIGGER);
        animator.SetTrigger(HIDE_TRIGGER);
    }

    private void GeneralWavesManager_OnWaveStart(object sender, GeneralWavesManager.OnWaveEventArgs e)
    {
        SetWaveText(e.waveNumber);
        ShowUI();
    }

    private void GeneralWavesManager_OnWaveCompleted(object sender, GeneralWavesManager.OnWaveEventArgs e)
    {
        HideUI();
    }
}
