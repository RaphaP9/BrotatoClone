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

    [Header("Suffix")]
    [SerializeField] private bool addTutorialSuffix;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private void OnEnable()
    {
        GameManager.OnStateChanged += GameManager_OnStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnStateChanged -= GameManager_OnStateChanged;
    }

    private void SetWaveText(int waveNumber)
    {
        if(addTutorialSuffix) waveText.text = $"Tutorial - Oleada {waveNumber}";
        else waveText.text = $"Oleada {waveNumber}";
    }

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

    private void GameManager_OnStateChanged(object sender, GameManager.OnStateEventArgs e)
    {
        if(e.newState == GameManager.State.StartingWave)
        {
            SetWaveText(GeneralWavesManager.Instance.CurrentWaveNumber);
            ShowUI();
        }

        if(e.newState == GameManager.State.EndingWave)
        {
            HideUI();
        }
    }
}
