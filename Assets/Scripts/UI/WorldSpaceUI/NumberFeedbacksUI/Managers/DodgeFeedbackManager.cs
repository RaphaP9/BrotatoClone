using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeFeedbackManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform dodgeUIPrefab;

    [Header("Settings")]
    [SerializeField, Range(-2f, 2f)] private float YOffset;
    [SerializeField, Range(0f, 3f)] private float maxXOffset;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDodge += PlayerHealth_OnPlayerDodge;
        EnemyHealth.OnEnemyDodge += EnemyHealth_OnEnemyDodge;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDodge -= PlayerHealth_OnPlayerDodge;
        EnemyHealth.OnEnemyDodge -= EnemyHealth_OnEnemyDodge;
    }

    private void CreateDodgeFeedback(Transform prefab, Vector2 position)
    {
        Transform feedbackTransform = Instantiate(prefab, GeneralUtilities.Vector2ToVector3(position), Quaternion.identity);

        DodgeUI dodgeUI = feedbackTransform.GetComponentInChildren<DodgeUI>();

        if (dodgeUI == null)
        {
            if (debug) Debug.Log("Instantiated feedback does not contain a DamageTakenUI component.");
            return;
        }
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

    private void PlayerHealth_OnPlayerDodge(object sender, System.EventArgs e)
    {
        Vector2 instantiationPosition = GetInstantiationPosition((sender as PlayerHealth).transform);
        CreateDodgeFeedback(dodgeUIPrefab, instantiationPosition);

    }

    private void EnemyHealth_OnEnemyDodge(object sender, System.EventArgs e)
    {
        Vector2 instantiationPosition = GetInstantiationPosition((sender as EnemyHealth).transform);
        CreateDodgeFeedback(dodgeUIPrefab, instantiationPosition);

    }
    #endregion
}
