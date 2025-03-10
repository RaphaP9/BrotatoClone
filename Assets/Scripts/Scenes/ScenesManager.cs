using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance { get; private set; }

    [Header("States")]
    [SerializeField] private State state;
    public enum State { Idle, TransitionOut, FullBlack, TransitionIn }

    public State SceneState => state;

    [Header("Settings")]
    [SerializeField, Range(0.1f, 0.5f)] private float fadeInInterval;

    public static event EventHandler<OnSceneLoadEventArgs> OnSceneTransitionOutStart;
    public static event EventHandler<OnSceneLoadEventArgs> OnSceneTransitionInStart;
    public static event EventHandler<OnSceneLoadEventArgs> OnSceneLoadStart;
    public static event EventHandler<OnSceneLoadEventArgs> OnSceneLoad;

    private string sceneToLoad;

    public class OnSceneLoadEventArgs : EventArgs
    {
        public string originScene;
        public string targetScene;
    }

    private void OnEnable()
    {
        SceneTransitionUIHandler.OnFadeOutEnd += TransitionUIHandler_OnFadeOutEnd;
        SceneTransitionUIHandler.OnFadeInEnd += TransitionUIHandler_OnFadeInEnd;
    }

    private void OnDisable()
    {
        SceneTransitionUIHandler.OnFadeOutEnd -= TransitionUIHandler_OnFadeOutEnd;
        SceneTransitionUIHandler.OnFadeInEnd -= TransitionUIHandler_OnFadeInEnd;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        OnSceneLoad?.Invoke(this, new OnSceneLoadEventArgs { originScene = "", targetScene = SceneManager.GetActiveScene().name });
        OnSceneTransitionInStart.Invoke(this, new OnSceneLoadEventArgs { originScene = "", targetScene = SceneManager.GetActiveScene().name });
        SetSceneState(State.TransitionIn);
    }

    private void SetSceneState(State state) => this.state = state;

    private bool CanChangeScene() => state == State.Idle;

    #region SimpleLoad
    public void SimpleReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SimpleLoadTargetScene(currentSceneName);
    }

    public void SimpleLoadTargetScene(string targetScene)
    {
        if (!CanChangeScene()) return;
        StartCoroutine(LoadSceneCoroutine(targetScene));
    }
    #endregion

    #region FadeLoad

    public void FadeLoadTargetScene(string targetScene)
    {
        if (!CanChangeScene()) return;

        string originScene = SceneManager.GetActiveScene().name;

        SetSceneState(State.TransitionOut);
        OnSceneTransitionOutStart?.Invoke(this, new OnSceneLoadEventArgs { originScene = originScene, targetScene = targetScene });
        sceneToLoad = targetScene;
    }

    public void FadeReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        FadeLoadTargetScene(currentSceneName);
    }

    private IEnumerator LoadSceneTransitionCoroutine(string targetScene)
    {
        string originScene = SceneManager.GetActiveScene().name;

        yield return StartCoroutine(LoadSceneCoroutine(targetScene));

        yield return new WaitForSeconds(0.1f);
        OnSceneTransitionInStart?.Invoke(this, new OnSceneLoadEventArgs { originScene = originScene, targetScene = targetScene });

        SetSceneState(State.TransitionIn);

        sceneToLoad = "";
    }

    #endregion

    public void LoadScene(string targetScene)
    {
        string originScene = SceneManager.GetActiveScene().name;

        SceneManager.LoadSceneAsync(targetScene);
        OnSceneLoad?.Invoke(this, new OnSceneLoadEventArgs { originScene = originScene, targetScene = targetScene });
    }

    private IEnumerator LoadSceneCoroutine(string targetScene)
    {
        string originScene = SceneManager.GetActiveScene().name;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene);

        OnSceneLoadStart?.Invoke(this, new OnSceneLoadEventArgs { originScene = originScene, targetScene = targetScene });

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        OnSceneLoad?.Invoke(this, new OnSceneLoadEventArgs { originScene = originScene, targetScene = targetScene });
    }

    public void QuitGame() => Application.Quit();

    #region TransitionUIHandler Subscriptions
    private void TransitionUIHandler_OnFadeOutEnd(object sender, EventArgs e)
    {
        SetSceneState(State.FullBlack);
        StartCoroutine(LoadSceneTransitionCoroutine(sceneToLoad));
    }
    private void TransitionUIHandler_OnFadeInEnd(object sender, EventArgs e)
    {
        SetSceneState(State.Idle);
    }
    #endregion

}
