using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyKamikaze : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemyIdentifier enemyIdentifier;
    [SerializeField] private EnemyPlayerDetector enemyPlayerDetector;
    [SerializeField] private EnemySpawningHandler enemySpawningHandler;
    [SerializeField] private EnemyHealth enemyHealth;

    [Header("Settings")]
    [SerializeField] private LayerMask playerLayerMask;

    public static event EventHandler OnEnemySelfDestroy;
    public event EventHandler OnThisEnemySelfDestroy;

    public class OnEnemySelfDestroyEventArgs : EventArgs
    {
        public EnemySO enemySO;
        public int damage;
    }

    private void OnEnable()
    {
        enemyPlayerDetector.OnPlayerDetected += EnemyPlayerDetector_OnPlayerDetected;
    }

    private void OnDisable()
    {
        enemyPlayerDetector.OnPlayerDetected -= EnemyPlayerDetector_OnPlayerDetected;
    }

    private bool CanSelfDestroy()
    {
        if (!enemyHealth.IsAlive()) return false;
        if (enemySpawningHandler.IsSpawning) return false;

        return true;
    }

    private void SelfDestroy()
    {
        int kamikazeDamage = enemyIdentifier.EnemySO.kamikazeDamage;
        float kamikazeDamageRange = enemyIdentifier.EnemySO.kamikazeDamageRange;

        List<Vector2> positions = new List<Vector2>();
        Vector2 position = GeneralUtilities.TransformPositionVector2(transform);
        positions.Add(position);

        GeneralGameplayUtilities.DealRegularDamageInArea(kamikazeDamage, positions , kamikazeDamageRange, true, playerLayerMask, enemyIdentifier.EnemySO);

        OnEnemySelfDestroy?.Invoke(this, new OnEnemySelfDestroyEventArgs { enemySO = enemyIdentifier.EnemySO, damage = kamikazeDamage });
        OnThisEnemySelfDestroy?.Invoke(this, new OnEnemySelfDestroyEventArgs { enemySO = enemyIdentifier.EnemySO, damage = kamikazeDamage });
    }

    private void EnemyPlayerDetector_OnPlayerDetected(object sender, System.EventArgs e)
    {
        if (!CanSelfDestroy()) return;
        SelfDestroy();
    }
}
