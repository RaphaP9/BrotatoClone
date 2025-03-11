using System;
using UnityEngine;

public class NewMovementInput : MovementInput
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
        playerInputActions.Movement.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Movement.Disable();
    }


    public override bool CanProcessInput()
    {
        //if (GameManager.Instance.GameState != GameManager.State.OnGameplay) return false;
        return true;
    }

    public override Vector2 GetMovementInputNormalized()
    {
        if (!CanProcessInput()) return Vector2.zero;

        Vector2 input = playerInputActions.Movement.Move.ReadValue<Vector2>();

        return input;
    }

    public override bool GetJumpDown()
    {
        if (!CanProcessInput()) return false;

        bool input = playerInputActions.Movement.Jump.WasPerformedThisFrame();

        return input;
    }

    public override bool GetJump()
    {
        if (!CanProcessInput()) return false;

        bool input = playerInputActions.Movement.Jump.IsPressed();

        return input;
    }

    public override bool GetDashDown()
    {
        if (!CanProcessInput()) return false;

        bool input = playerInputActions.Movement.Dash.WasPerformedThisFrame();

        return input;
    }
}
