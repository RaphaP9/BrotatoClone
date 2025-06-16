using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIButtonsHandler : MonoBehaviour
{
    [Header("Play")]
    [SerializeField] private Button playButton;
    [SerializeField] private string playScene;

    [Header("Play")]
    [SerializeField] private Button tutorialButton;
    [SerializeField] private string tutorialScene;

    [Header("Credits")]
    [SerializeField] private Button creditsButton;
    [SerializeField] private string creditsScene;

    [Header("Quit")]
    [SerializeField] private Button quitButton;

    [Header("Cinematic")]
    [SerializeField] private Button cinematicButton;
    [SerializeField] private string cinematicScene;

    private void Awake()
    {
        InitializeButtonsListeners();
    }
    
    private void InitializeButtonsListeners()
    {
        playButton.onClick.AddListener(LoadPlayScene);
        tutorialButton.onClick.AddListener(LoadTutorialScene);
        creditsButton.onClick.AddListener(LoadCreditsScene);
        quitButton.onClick.AddListener(QuitGame);

        cinematicButton.onClick.AddListener(LoadCinematicScene);
    }


    private void LoadPlayScene() => ScenesManager.Instance.FadeLoadTargetScene(playScene);
    private void LoadTutorialScene() => ScenesManager.Instance.FadeLoadTargetScene(tutorialScene);
    private void LoadCreditsScene() => ScenesManager.Instance.FadeLoadTargetScene(creditsScene);
    private void QuitGame() => ScenesManager.Instance.QuitGame();

    private void LoadCinematicScene() => ScenesManager.Instance.FadeLoadTargetScene(cinematicScene);
}
