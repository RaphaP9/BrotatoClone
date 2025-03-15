using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyKamikaze : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemyIdentifier enemyIdentifier;
    [SerializeField] private EnemyPlayerKamikazeDetector enemyPlayerDetector;
    [SerializeField] private EnemySpawningHandler enemySpawningHandler;
    [SerializeField] private EnemyHealth enemyHealth;

    [Header("Settings")]
    [SerializeField] private LayerMask playerLayerMask;

    public static event EventHandler<OnEnemyExplosionEventArgs> OnEnemySelfDestroyCompleted;
    public event EventHandler<OnEnemyExplosionEventArgs> OnThisEnemySelfDestroyCompleted;

    public static event EventHandler<OnEnemyExplosionEventArgs> OnEnemySelfDestroyBegin;
    public event EventHandler<OnEnemyExplosionEventArgs> OnThisEnemySelfDestroyBegin;

    public static event EventHandler<OnEnemyExplosionEventArgs> OnEnemyExplosion;
    public event EventHandler<OnEnemyExplosionEventArgs> OnThisEnemyExplosion;

    private KamikazeEnemySO KamikazeEnemySO => enemyIdentifier.EnemySO as KamikazeEnemySO;
    public bool IsExploding { get; private set; } = false;
    public bool HasExplodedKamikaze { get; private set; } = false;

    public class OnEnemyExplosionEventArgs : EventArgs
    {
        public EnemySO enemySO;
        public int damage;
    }

    private void OnEnable()
    {
        enemyPlayerDetector.OnPlayerDetected += EnemyPlayerDetector_OnPlayerDetected;
        enemyHealth.OnThisEnemyDeath += EnemyHealth_OnThisEnemyDeath;
    }

    private void OnDisable()
    {
        enemyPlayerDetector.OnPlayerDetected -= EnemyPlayerDetector_OnPlayerDetected;
        enemyHealth.OnThisEnemyDeath -= EnemyHealth_OnThisEnemyDeath;
    }

    private bool CanSelfDestroy()
    {
        if (IsExploding) return false;
        if (!enemyHealth.IsAlive()) return false;
        if (enemySpawningHandler.IsSpawning) return false;

        return true;
    }

    private IEnumerator KamikazeCoroutine()
    {
        IsExploding = true;

        OnEnemySelfDestroyBegin?.Invoke(this, new OnEnemyExplosionEventArgs { enemySO = enemyIdentifier.EnemySO, damage = KamikazeEnemySO.kamikazeRegularDamage });
        OnThisEnemySelfDestroyBegin?.Invoke(this, new OnEnemyExplosionEventArgs { enemySO = enemyIdentifier.EnemySO, damage = KamikazeEnemySO.kamikazeRegularDamage });

        yield return new WaitForSeconds(KamikazeEnemySO.kamikazeExplosionTime);
        KamikazeSelfDestroy();
    }

    private void KamikazeSelfDestroy()
    {
        DealExplosionDamage();
        HasExplodedKamikaze = true;
        IsExploding = false;

        OnEnemySelfDestroyCompleted?.Invoke(this, new OnEnemyExplosionEventArgs { enemySO = enemyIdentifier.EnemySO, damage = KamikazeEnemySO.kamikazeRegularDamage });
        OnThisEnemySelfDestroyCompleted?.Invoke(this, new OnEnemyExplosionEventArgs { enemySO = enemyIdentifier.EnemySO, damage = KamikazeEnemySO.kamikazeRegularDamage });

        enemyHealth.InstaKill();
    }

    private void DealExplosionDamage()
    {
        int kamikazeRegularDamage = KamikazeEnemySO.kamikazeRegularDamage;

        int kamikazeBleedDamage = KamikazeEnemySO.kamikazeBleedDamage;
        float kamikazeBleedDuration = KamikazeEnemySO.kamikazeBleedDuration;
        float kamikazeBleedTickTime = KamikazeEnemySO.kamikazeBleedTickTime;

        float kamikazeDamageRange = KamikazeEnemySO.kamikazeDamageRange;

        List<Vector2> positions = new List<Vector2>();
        Vector2 position = GeneralUtilities.TransformPositionVector2(transform);
        positions.Add(position);

        if (HasKamikazeRegularDamage())
        {
            GeneralGameplayUtilities.DealRegularDamageInArea(kamikazeRegularDamage, positions, kamikazeDamageRange, true, playerLayerMask, enemyIdentifier.EnemySO);
        }

        if (HasKamikazeBleedDamage())
        {
            GeneralGameplayUtilities.DealBleedDamageInArea(kamikazeBleedDamage, kamikazeBleedDuration, kamikazeBleedTickTime, positions, kamikazeDamageRange, true, playerLayerMask, enemyIdentifier.EnemySO);
        }

        OnEnemyExplosion?.Invoke(this, new OnEnemyExplosionEventArgs { enemySO = enemyIdentifier.EnemySO, damage = KamikazeEnemySO.kamikazeRegularDamage });
        OnThisEnemyExplosion?.Invoke(this, new OnEnemyExplosionEventArgs { enemySO = enemyIdentifier.EnemySO, damage = KamikazeEnemySO.kamikazeRegularDamage });
    }

    private bool HasKamikazeRegularDamage() => KamikazeEnemySO.kamikazeRegularDamage > 0;
    private bool HasKamikazeBleedDamage() => KamikazeEnemySO.kamikazeBleedDamage > 0;

    private void EnemyPlayerDetector_OnPlayerDetected(object sender, System.EventArgs e)
    {
        if (!CanSelfDestroy()) return;

        IsExploding = true;
        StartCoroutine(KamikazeCoroutine());
    }

    private void EnemyHealth_OnThisEnemyDeath(object sender, EventArgs e)
    {
        StopAllCoroutines();
        if (HasExplodedKamikaze) return;

        DealExplosionDamage();
    }
}
