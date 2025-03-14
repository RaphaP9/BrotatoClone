using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponAim : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector2 aimDirection;

    public Vector2 AimDirection => aimDirection;
    public bool FacingRight {  get; private set; }

    private const float FACING_RIGHT_UPPER_LIMIT = 90f;
    private const float FACING_RIGHT_LOWER_LIMIT = -90f;

    private void Update()
    {
        HandleAim();
    }

    private void HandleAim()
    {
        if (!CanAim()) return;

        UpdateAimDirection();

        float aimAngle = CalculateAimAngle();
        SetZRotation(aimAngle);

        FacingRight = CheckFacingRight(aimAngle);
    }

    private bool CanAim()
    {
        if (!PlayerHealth.Instance.IsAlive()) return false;

        return true;
    }

    private void UpdateAimDirection()
    {
        aimDirection = CalculateAimUnitaryVector();
    }

    private float CalculateAimAngle()
    {
        Vector2 normalizedAimVector = CalculateAimUnitaryVector();
        float aimAngle = GeneralUtilities.GetVector2AngleDegrees(normalizedAimVector);

        return aimAngle;
    }

    private Vector2 CalculateAimUnitaryVector()
    {
        Vector2 aimVector = ScreenInput.Instance.GetWorldMousePosition() - GeneralUtilities.TransformPositionVector2(transform);
        aimVector.Normalize();

        return aimVector;
    }

    private void SetZRotation(float angle)
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private bool CheckFacingRight(float angle)
    {
        if(angle > FACING_RIGHT_UPPER_LIMIT ) return false;
        if(angle < FACING_RIGHT_LOWER_LIMIT ) return false;

        return true;
    }
}
