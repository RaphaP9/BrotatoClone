using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestScript : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LayerMask layerMask;

    private void OnEnable()
    {
        PlayerHealth.OnPlayerTakeBleedDamage += PlayerHealth_OnPlayerTakeBleedDamage;
        PlayerHealth.OnPlayerTakeRegularDamage += PlayerHealth_OnPlayerTakeRegularDamage;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerTakeBleedDamage -= PlayerHealth_OnPlayerTakeBleedDamage;
        PlayerHealth.OnPlayerTakeRegularDamage -= PlayerHealth_OnPlayerTakeRegularDamage;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            GeneralGameplayUtilities.DealBleedDamageInArea(2,4,1, transform.position, 1f, true, layerMask);
        }
    }


    private void PlayerHealth_OnPlayerTakeBleedDamage(object sender, EntityHealth.OnEntityTakeDamageEventArgs e)
    {
        Debug.Log($"Bleeding: {e.damageTaken}, IsCrit {e.isCrit}");
    }
    private void PlayerHealth_OnPlayerTakeRegularDamage(object sender, EntityHealth.OnEntityTakeDamageEventArgs e)
    {
        Debug.Log($"Damage: {e.damageTaken}, IsCrit {e.isCrit}");

    }
}
