using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTakenFeedbackManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform regularDamageTakenUIPrefab;
    [SerializeField] private Transform bleedDamageTakenUIPrefab;

    [Header("Settings")]
    [SerializeField, Range(-2f, 2f)] private float YOffset;
    [SerializeField, Range(0f, 3f)] private float maxXOffset;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private void OnEnable()
    {
        PlayerHealth.OnPlayerTakeRegularDamage += PlayerHealth_OnPlayerTakeRegularDamage;
        PlayerHealth.OnPlayerTakeBleedDamage += PlayerHealth_OnPlayerTakeBleedDamage;

        EnemyHealth.OnEnemyTakeRegularDamage += EnemyHealth_OnEnemyTakeRegularDamage;
        EnemyHealth.OnEnemyTakeBleedDamage += EnemyHealth_OnEnemyTakeBleedDamage;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerTakeRegularDamage -= PlayerHealth_OnPlayerTakeRegularDamage;
        PlayerHealth.OnPlayerTakeBleedDamage -= PlayerHealth_OnPlayerTakeBleedDamage;

        EnemyHealth.OnEnemyTakeRegularDamage -= EnemyHealth_OnEnemyTakeRegularDamage;
        EnemyHealth.OnEnemyTakeBleedDamage -= EnemyHealth_OnEnemyTakeBleedDamage;
    }

    private void CreateDamageFeedback(Transform prefab, Vector2 position, int damage, Color damageColor, bool isCrit)
    {
        Transform feedbackTransform = Instantiate(prefab, GeneralUtilities.Vector2ToVector3(position), Quaternion.identity);

        DamageTakenUI damageTakenUI = feedbackTransform.GetComponentInChildren<DamageTakenUI>();

        if(damageTakenUI == null)
        {
            if (debug) Debug.Log("Instantiated feedback does not contain a damageTakenUI.");
            return;
        }

        damageTakenUI.SetDamageUI(damage,damageColor,isCrit);
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
    private void PlayerHealth_OnPlayerTakeRegularDamage(object sender, EntityHealth.OnEntityTakeDamageEventArgs e)
    {
        Vector2 instantiationPosition = GetInstantiationPosition(e.entityHealth.transform);
        CreateDamageFeedback(regularDamageTakenUIPrefab, instantiationPosition, e.damageTaken, e.damageSource.GetDamageColor(), e.isCrit);
    }

    private void PlayerHealth_OnPlayerTakeBleedDamage(object sender, EntityHealth.OnEntityTakeDamageEventArgs e)
    {
        Vector2 instantiationPosition = GetInstantiationPosition(e.entityHealth.transform);
        CreateDamageFeedback(bleedDamageTakenUIPrefab, instantiationPosition, e.damageTaken, e.damageSource.GetDamageColor(), e.isCrit);
    }

    private void EnemyHealth_OnEnemyTakeRegularDamage(object sender, EnemyHealth.OnEnemyTakeDamageEventArgs e)
    {
        Vector2 instantiationPosition = GetInstantiationPosition(e.entityHealth.transform);
        CreateDamageFeedback(regularDamageTakenUIPrefab, instantiationPosition, e.damageTaken, e.damageSource.GetDamageColor(), e.isCrit);
    }

    private void EnemyHealth_OnEnemyTakeBleedDamage(object sender, EnemyHealth.OnEnemyTakeDamageEventArgs e)
    {
        Vector2 instantiationPosition = GetInstantiationPosition(e.entityHealth.transform);
        CreateDamageFeedback(bleedDamageTakenUIPrefab, instantiationPosition, e.damageTaken, e.damageSource.GetDamageColor(), e.isCrit);
    }
    #endregion
}
