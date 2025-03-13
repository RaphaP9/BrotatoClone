using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewScreenInput : ScreenInput
{
    [Header("Components")]
    [SerializeField] private Camera _camera;

    public override bool CanProcessInput()
    {
        return true;
    }

    public override Vector2 GetWorldMousePosition()
    {
        if(!CanProcessInput()) return Vector2.zero;

        Vector3 rawPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 screenPosition = GeneralUtilities.SupressZComponent(rawPosition);

        return screenPosition;
    }
}
