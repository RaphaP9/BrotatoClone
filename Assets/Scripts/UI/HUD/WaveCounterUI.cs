using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveCounterUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI waveCounterText;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private int previousCounter;
    private bool enableCounterUpdate;

    private void OnEnable()
    {
        WaveSpawningSystemManager.OnWaveStart += WaveSpawningSystemManager_OnWaveStart;
        WaveSpawningSystemManager.OnWaveCompleted += WaveSpawningSystemManager_OnWaveCompleted;
    }

    private void OnDisable()
    {
        WaveSpawningSystemManager.OnWaveStart -= WaveSpawningSystemManager_OnWaveStart;
        WaveSpawningSystemManager.OnWaveCompleted -= WaveSpawningSystemManager_OnWaveCompleted;
    }

    private void Start()
    {
        ResetPreviousCounter();
    }

    private void Update()
    {
        HandleCounter();
    }

    private void HandleCounter()
    {
        if (!enableCounterUpdate) return;
        int currentCounter = Mathf.CeilToInt(WaveSpawningSystemManager.Instance.CurrentWaveDuration - WaveSpawningSystemManager.Instance.CurrentWaveElapsedTime);

        if (currentCounter == previousCounter) return;
        if (currentCounter <= 0) return; //Don't update counter to 0, keep countdown on 1 at minimum

        SetCounterText(currentCounter);

        previousCounter = currentCounter;
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

    private void ResetPreviousCounter() => previousCounter = 0;
    private void SetCounterText(int counter) => waveCounterText.text = counter.ToString();

    private void WaveSpawningSystemManager_OnWaveStart(object sender, WaveSpawningSystemManager.OnWaveEventArgs e)
    {
        ShowUI();
        enableCounterUpdate = true;

    }

    private void WaveSpawningSystemManager_OnWaveCompleted(object sender, WaveSpawningSystemManager.OnWaveEventArgs e)
    {
        HideUI();
        enableCounterUpdate = false;
    }


}
