using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAttackInput : AttackInput
{
    private PlayerInputActions playerInputActions;

    protected override void Awake()
    {
        base.Awake();
        InitializePlayerInputActions();
    }

    private void InitializePlayerInputActions()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Attacking.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Attacking.Disable();
    }

    public override bool CanProcessInput()
    {
        if (GameManager.Instance.GameState == GameManager.State.StartingGame && GameManager.Instance.AllowActionsWhileStartingGame) return true;
        if (GameManager.Instance.GameState == GameManager.State.StartingWave) return true;
        if (GameManager.Instance.GameState == GameManager.State.Wave) return true;
        if (GameManager.Instance.GameState == GameManager.State.EndingWave) return true;

        return false;
    }

    public override bool GetAttackDown()
    {
        if (!CanProcessInput()) return false;

        bool input = playerInputActions.Attacking.Attack.WasPerformedThisFrame();

        return input;
    }

    public override bool GetAttackHold()
    {
        if (!CanProcessInput()) return false;

        bool input = playerInputActions.Attacking.Attack.IsPressed();

        return input;
    }
}
