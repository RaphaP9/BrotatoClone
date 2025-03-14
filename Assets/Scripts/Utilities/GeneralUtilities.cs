using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneralUtilities
{
    #region Vectors
    public static Vector2 SupressZComponent(Vector3 vector3) => new Vector2(vector3.x, vector3.y);

    public static float GetVector2AngleDegrees(Vector2 vector2) => Mathf.Atan2(vector2.y, vector2.x) * Mathf.Rad2Deg;
    #endregion

    #region Floats
    public static float RoundToNDecimalPlaces(float number, int decimalPlaces) => Mathf.Round(number * Mathf.Pow(10, decimalPlaces)) / Mathf.Pow(10, decimalPlaces);

    public static float ClampNumber01(float number) => Mathf.Clamp01(number);

    #endregion

    #region Transforms
    public static Vector2 TransformPositionVector2(Transform transform) => new Vector2(transform.position.x, transform.position.y);

    public static List<Transform> GetTransformsByColliders(Collider2D[] colliders)
    {
        List<Transform> transforms = new List<Transform>();

        foreach (Collider2D collider in colliders)
        {
            Transform transform = GetTransformByCollider(collider);

            transforms.Add(transform);
        }

        return transforms;
    }

    public static Transform GetTransformByCollider(Collider2D collider) => collider.transform;
    #endregion  
}
