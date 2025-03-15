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

    private void SelfDestroy()
    {
        int kamikazeDamage = enemyIdentifier.EnemySO.kamikazeDamage;
        float kamikazeDamageRange = enemyIdentifier.EnemySO.kamikazeDamageRange;

        GeneralGameplayUtilities.DealRegularDamageInArea(kamikazeDamage, transform.position, kamikazeDamageRange, true, playerLayerMask, enemyIdentifier.EnemySO);

        OnEnemySelfDestroy?.Invoke(this, new OnEnemySelfDestroyEventArgs { enemySO = enemyIdentifier.EnemySO, damage = kamikazeDamage });
        OnThisEnemySelfDestroy?.Invoke(this, new OnEnemySelfDestroyEventArgs { enemySO = enemyIdentifier.EnemySO, damage = kamikazeDamage });
    }

    private void EnemyPlayerDetector_OnPlayerDetected(object sender, System.EventArgs e)
    {
        if (!enemyHealth.IsAlive()) return;
        SelfDestroy();
    }
}
