using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestScript : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private WeaponSO testWeapon;

    private void OnEnable()
    {
        PlayerHealth.OnPlayerTakeBleedDamage += PlayerHealth_OnPlayerTakeBleedDamage;
        PlayerHealth.OnPlayerTakeRegularDamage += PlayerHealth_OnPlayerTakeRegularDamage;

        EnemyHealth.OnEnemyDodge += EnemyHealth_OnEnemyDodge;
    }



    private void OnDisable()
    {
        PlayerHealth.OnPlayerTakeBleedDamage -= PlayerHealth_OnPlayerTakeBleedDamage;
        PlayerHealth.OnPlayerTakeRegularDamage -= PlayerHealth_OnPlayerTakeRegularDamage;

        EnemyHealth.OnEnemyDodge -= EnemyHealth_OnEnemyDodge;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            GeneralGameplayUtilities.DealBleedDamageInArea(2,4,1, transform.position, 1f, true, layerMask, testWeapon);
        }
    }


    private void PlayerHealth_OnPlayerTakeBleedDamage(object sender, EntityHealth.OnEntityTakeDamageEventArgs e)
    {
        Debug.Log($"Bleeding: {e.damageTaken}, IsCrit {e.isCrit}, From: {e.damageSource.GetName()}");
    }
    private void PlayerHealth_OnPlayerTakeRegularDamage(object sender, EntityHealth.OnEntityTakeDamageEventArgs e)
    {
        Debug.Log($"Damage: {e.damageTaken}, IsCrit {e.isCrit}, From: {e.damageSource.GetName()}");

    }
    private void EnemyHealth_OnEnemyDodge(object sender, System.EventArgs e)
    {
        Debug.Log("EnemyDodged!");
    }
}
