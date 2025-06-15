using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseSceneUIButtonsHandler : MonoBehaviour
{
    [Header("Retry")]
    [SerializeField] private Button retryButton;
    [SerializeField] private string retryScene;

    [Header("Back")]
    [SerializeField] private Button backButton;
    [SerializeField] private string backScene;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        retryButton.onClick.AddListener(LoadRetryScene);
        backButton.onClick.AddListener(LoadBackScene);
    }

    private void LoadRetryScene() => ScenesManager.Instance.FadeLoadTargetScene(retryScene);
    private void LoadBackScene() => ScenesManager.Instance.FadeLoadTargetScene(backScene);
}
