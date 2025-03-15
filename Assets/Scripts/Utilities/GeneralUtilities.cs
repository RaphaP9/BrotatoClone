using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public static class GeneralUtilities
{
    #region Vectors
    public static Vector2 SupressZComponent(Vector3 vector3) => new Vector2(vector3.x, vector3.y);

    public static float GetVector2AngleDegrees(Vector2 vector2) => Mathf.Atan2(vector2.y, vector2.x) * Mathf.Rad2Deg;

    public static Vector3 Vector2ToVector3(Vector2 vector2) => new Vector3(vector2.x, vector2.y, 0f );

    public static Vector2 RotateVector2ByAngleDegrees(Vector2 vector, float angleDegrees)
    {
        float magnitude = vector.magnitude;

        float angleRadians = angleDegrees * Mathf.Deg2Rad;

        float rotationSin = Mathf.Sin(angleRadians);
        float rotationCos = Mathf.Cos(angleRadians);

        Vector2 rotatedVector = new Vector2(vector.x * rotationCos - vector.y * rotationSin, vector.x * rotationSin + vector.y * rotationCos);
        rotatedVector.Normalize();

        Vector2 finalVector = rotatedVector * magnitude;

        return finalVector;

    }
    #endregion

    #region Floats
    public static float RoundToNDecimalPlaces(float number, int decimalPlaces) => Mathf.Round(number * Mathf.Pow(10, decimalPlaces)) / Mathf.Pow(10, decimalPlaces);

    public static float ClampNumber01(float number) => Mathf.Clamp01(number);

    #endregion

    #region Transforms
    public static Vector2 TransformPositionVector2(Transform transform) => new Vector2(transform.position.x, transform.position.y);

    public static void RotateTransformTowardsVector2(Transform transform, Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public static List<Vector2> TransformPositionVector2List(List<Transform> transforms)
    {
        List<Vector2> vectorList = new List<Vector2>();

        foreach (Transform transform in transforms)
        {
            vectorList.Add(TransformPositionVector2(transform));
        }

        return vectorList;
    }

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

    #region LayerMasks
    public static bool CheckGameObjectInLayerMask(GameObject gameObject, LayerMask layerMask) => ((1 << gameObject.layer) & layerMask) != 0;
    #endregion
}
