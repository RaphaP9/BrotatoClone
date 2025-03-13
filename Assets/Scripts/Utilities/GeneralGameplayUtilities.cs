using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneralGameplayUtilities
{
    public const float PERSPECTIVE_SCALE_X = 1f;
    public const float PERSPECTIVE_SCALE_Y = 1f;

    public static Vector2 ScaleVector2ToPerspective(Vector2 baseVector)
    {
        Vector2 scaledVector = new Vector2(baseVector.x * PERSPECTIVE_SCALE_X, baseVector.y *PERSPECTIVE_SCALE_Y);
        return scaledVector;
    }
}
