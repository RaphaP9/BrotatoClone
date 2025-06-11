using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldDroppedFeedbackManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform goldDroppedUIPrefab;

    [Header("Settings")]
    [SerializeField, Range(-2f, 2f)] private float YOffset;
    [SerializeField, Range(0f, 3f)] private float maxXOffset;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private void OnEnable()
    {
        GoldDropperManager.OnEntityDropGold += GoldDropperManager_OnEntityDropGold;
    }

    private void OnDisable()
    {
        GoldDropperManager.OnEntityDropGold -= GoldDropperManager_OnEntityDropGold;
    }

    private void CreateGoldFeedback(Transform prefab, Vector2 position, int gold)
    {
        Transform feedbackTransform = Instantiate(prefab, GeneralUtilities.Vector2ToVector3(position), Quaternion.identity);

        GoldDroppedUI goldDroppedUI = feedbackTransform.GetComponentInChildren<GoldDroppedUI>();

        if (goldDroppedUI == null)
        {
            if (debug) Debug.Log("Instantiated feedback does not contain a GoldDroppedUI component.");
            return;
        }

        goldDroppedUI.SetGoldUI(gold);
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

    private void GoldDropperManager_OnEntityDropGold(object sender, GoldDropperManager.OnEntityDropGoldEventArgs e)
    {
        CreateGoldFeedback(goldDroppedUIPrefab, e.entityPosition, e.goldAmount);
    }
    #endregion
}
