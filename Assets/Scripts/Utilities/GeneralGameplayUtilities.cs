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

    #region DamageTakenProcessing
    public static bool EvaluateDodgeChance(float dodgeChance)
    {
        float randomNumber = Random.Range(0f, 1f);

        if (dodgeChance >= randomNumber) return true;
        return false;
    }

    public static int MitigateDamageByPercentage(int baseDamage, float armor)
    {
        float clampedArmor = GeneralUtilities.ClampNumber01(armor);
        float resultingDamage = baseDamage * (1- clampedArmor);

        int roundedDamage = Mathf.RoundToInt(resultingDamage);

        return roundedDamage;
    }

    #endregion
}
