using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsUIButtonsHandler : MonoBehaviour
{
    [Header("Play")]
    [SerializeField] private Button backButton;
    [SerializeField] private string backScene;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        backButton.onClick.AddListener(LoadBackScene);
    }

    private void LoadBackScene() => ScenesManager.Instance.FadeLoadTargetScene(backScene);
}
