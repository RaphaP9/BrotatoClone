using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDialogueInput : DialogueInput
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
        playerInputActions.Dialogue.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Dialogue.Disable();
    }

    public override bool CanProcessInput()
    {
        if (GameManager.Instance.GameState == GameManager.State.Dialogue) return true;
        
        return false;
    }

    public override bool GetSkipDown()
    {
        if (!CanProcessInput()) return false;

        bool input = playerInputActions.Dialogue.Skip.WasPerformedThisFrame();

        return input;

    }

}
