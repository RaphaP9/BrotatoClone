using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewScreenInput : ScreenInput
{
    [Header("Components")]
    [SerializeField] private Camera _camera;

    private Vector2 lastWorldMousePosition = new Vector2(1000f,0f);

    public override bool CanProcessInput()
    {
        if (PauseManager.Instance.GamePaused) return false;
        if (GameManager.Instance.GameState == GameManager.State.StartingGame && !GameManager.Instance.AllowActionsWhileStartingGame) return false;
        if ( GameManager.Instance.GameState == GameManager.State.Dialogue) return false;
        if( GameManager.Instance.GameState == GameManager.State.Lose) return false;
        if( GameManager.Instance.GameState == GameManager.State.Win) return false;

        return true;
    }

    public override Vector2 GetWorldMousePosition()
    {
        if(!CanProcessInput()) return lastWorldMousePosition;

        Vector3 rawPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 screenPosition = GeneralUtilities.SupressZComponent(rawPosition);

        lastWorldMousePosition = screenPosition;

        return screenPosition;
    }
}
