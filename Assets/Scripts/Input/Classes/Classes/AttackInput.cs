using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackInput : MonoBehaviour, IAttackInput
{
    public static AttackInput Instance { get; private set; }

    protected virtual void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one AttackInput instance");
        }

        Instance = this;
    }

    public abstract bool CanProcessInput();
    public abstract bool GetAttackDown();
    public abstract bool GetAttackHold();
}
