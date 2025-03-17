using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyPlayerKamikazeDetector : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemyIdentifier enemyIdentifier;
    [SerializeField] private CircleCollider2D circleCollider;

    [Header("Settings")]
    [SerializeField] private LayerMask playerLayerMask;

    public event EventHandler OnPlayerDetected;

    private KamikazeEnemySO KamikazeEnemySO => enemyIdentifier.EnemySO as KamikazeEnemySO;

    private void Start()
    {
        InitializeColliderSize();
    }

    private void InitializeColliderSize()
    {
        circleCollider.radius = KamikazeEnemySO.kamikazeDetectionRange;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!GeneralUtilities.CheckGameObjectInLayerMask(collision.gameObject, playerLayerMask)) return;

        OnPlayerDetected?.Invoke(this, EventArgs.Empty);
    }
}
