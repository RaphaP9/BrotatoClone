using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoldDropperManager : MonoBehaviour
{
    public static GoldDropperManager Instance { get; private set; }

    public static event EventHandler<OnEntityDropGoldEventArgs> OnEntityDropGold;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public class OnEntityDropGoldEventArgs : EventArgs
    {
        public int goldAmount;
        public Vector2 entityPosition;
    }        

    private void OnEnable()
    {
        EnemyHealth.OnEnemyDeath += EnemyHealth_OnEnemyDeath;
    }

    private void OnDisable()
    {
        EnemyHealth.OnEnemyDeath -= EnemyHealth_OnEnemyDeath;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one EntityGoldDropManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void DropEntityGoldAtPosition(int goldAmount, Vector2 entityPosition)
    {
        if (goldAmount <= 0) return;

        int goldDropped = GoldManager.Instance.AddGold(goldAmount);

        OnEntityDropGold?.Invoke(this, new OnEntityDropGoldEventArgs { goldAmount = goldDropped, entityPosition = entityPosition });
    }

    private void HandleGoldDrop(object sender, EnemyHealth.OnEnemyDeathEventArgs e)
    {
        if (e.damageSource.GetDamageDealerClassification() != DamageDealerClassification.Weapon) return;

        Vector2 position = GeneralUtilities.SupressZComponent((sender as EntityHealth).transform.position);

        DropEntityGoldAtPosition(e.enemySO.goldDrop, position);
    }

    #region Subscriptions
    private void EnemyHealth_OnEnemyDeath(object sender, EnemyHealth.OnEnemyDeathEventArgs e)
    {
        HandleGoldDrop(sender, e);
    }
    #endregion
}
