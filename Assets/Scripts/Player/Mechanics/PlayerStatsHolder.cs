using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsHolder : MonoBehaviour
{
    public static PlayerStatsHolder Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private PlayerStatsSO baseStats;

    public PlayerStatsSO BaseStats => baseStats;

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
            Debug.LogWarning("There is more than one PlayerStatsHolder instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

}
