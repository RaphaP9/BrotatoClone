using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WelcomeSceneManager : MonoBehaviour
{
    public static WelcomeSceneManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private string nextScene;

    public static event EventHandler OnAnyKeyPressed;

    private void Awake()
    {
        SetSingleton();
    }

    private void Update()
    {
        HandleAnyKeyPressed();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void HandleAnyKeyPressed()
    {
        if (ScenesManager.Instance.SceneState != ScenesManager.State.Idle) return;
        if (!Input.anyKeyDown) return;

        OnAnyKeyPressed?.Invoke(this, EventArgs.Empty);
        ScenesManager.Instance.FadeLoadTargetScene(nextScene);
    }
}
