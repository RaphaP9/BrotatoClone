using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealFeedbackManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform healUIPrefab;

    [Header("Settings")]
    [SerializeField, Range(-2f, 2f)] private float YOffset;
    [SerializeField, Range(0f, 3f)] private float maxXOffset;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private void OnEnable()
    {
        PlayerHealth.OnPlayerHeal += PlayerHealth_OnPlayerHeal;
        EnemyHealth.OnEnemyHeal += EnemyHealth_OnEnemyHeal;
    }
    private void OnDisable()
    {
        PlayerHealth.OnPlayerHeal -= PlayerHealth_OnPlayerHeal;
        EnemyHealth.OnEnemyHeal -= EnemyHealth_OnEnemyHeal;
    }

    private void CreateHealFeedback(Transform prefab, Vector2 position, int healAmount)
    {
        Transform feedbackTransform = Instantiate(prefab, GeneralUtilities.Vector2ToVector3(position), Quaternion.identity);

        HealUI healUI = feedbackTransform.GetComponentInChildren<HealUI>();

        if (healUI == null)
        {
            if (debug) Debug.Log("Instantiated feedback does not contain a HealUI component.");
            return;
        }

        healUI.SetHealUI(healAmount);
    }

    private Vector2 GetInstantiationPosition(Transform damagedTransform)
    {
        Vector2 offsetVector = new Vector2(GetRandomXOffset(), YOffset);
        Vector2 instantiationPosition = GeneralUtilities.TransformPositionVector2(damagedTransform) + offsetVector;
        return instantiationPosition;
    }

    private float GetRandomXOffset()
    {
        float xOffset = Random.Range(-maxXOffset, maxXOffset);
        return xOffset;
    }

    #region Subscriptions
    private void PlayerHealth_OnPlayerHeal(object sender, EntityHealth.OnEntityHealEventArgs e)
    {
        Vector2 instantiationPosition = GetInstantiationPosition(e.entityHealth.transform);
        CreateHealFeedback(healUIPrefab, instantiationPosition, e.healAmount);
    }

    private void EnemyHealth_OnEnemyHeal(object sender, EntityHealth.OnEntityHealEventArgs e)
    {
        Vector2 instantiationPosition = GetInstantiationPosition(e.entityHealth.transform);
        CreateHealFeedback(healUIPrefab, instantiationPosition, e.healAmount);
    }
    #endregion
}
