using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneralMethods
{
    public static Vector2 TransformPositionVector2(Transform transform) => new Vector2(transform.position.x, transform.position.y);
    public static Vector2 SupressZComponent(Vector3 vector3) => new Vector2(vector3.x, vector3.y);
    public static float RoundToNDecimalPlaces(float number, int decimalPlaces) => Mathf.Round(number * Mathf.Pow(10, decimalPlaces)) / Mathf.Pow(10, decimalPlaces);
}
