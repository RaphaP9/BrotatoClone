using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestScript : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;

    private void OnEnable()
    {
        PlayerHealth.OnPlayerTakeBleedDamage += PlayerHealth_OnPlayerTakeBleedDamage;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerTakeBleedDamage -= PlayerHealth_OnPlayerTakeBleedDamage;
    }

    private void PlayerHealth_OnPlayerTakeBleedDamage(object sender, EntityHealth.OnEntityTakeDamageEventArgs e)
    {
        Debug.Log($"Bleeding: {e.damageTaken}, IsCrit {e.isCrit}");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            playerHealth.Bleed(5,4,1, true);
        }
    }
}
