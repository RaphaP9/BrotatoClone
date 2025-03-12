using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScreenInput : MonoBehaviour
{
    public static ScreenInput Instance { get; private set; }

    protected virtual void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one ScreenInput instance");
        }

        Instance = this;
    }

    public abstract bool CanProcessInput();

    public abstract Vector2 GetWorldMousePosition();
}
