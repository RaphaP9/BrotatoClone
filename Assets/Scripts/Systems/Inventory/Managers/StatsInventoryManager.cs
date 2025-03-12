using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StatsInventoryManager : MonoBehaviour
{
    public static StatsInventoryManager Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<StatSO> statsInventory;

    [Header("Debug")]
    [SerializeField] private bool debug;
    
    public List<StatSO> StatsInventory => statsInventory;

    public static event EventHandler<OnStatEventArgs> OnStatAddedToInventory;
    public static event EventHandler<OnStatEventArgs> OnStatRemovedFromInventory;

    public class OnStatEventArgs : EventArgs
    {
        public StatSO statSO;
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {

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
            Debug.LogWarning("There is more than one StatsInventoryManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void AddStatToInventory(StatSO statSO)
    {
        if (statSO == null)
        {
            if (debug) Debug.Log("StatSO is null, addition will be ignored");
            return;
        }

        if (statsInventory.Contains(statSO))
        {
            if (debug) Debug.Log($"StatSO with name {statSO.inventoryObjectName} is already on inventory, addition will be ignored");
            return;
        }

        statsInventory.Add(statSO);

        OnStatAddedToInventory?.Invoke(this, new OnStatEventArgs { statSO = statSO });
    }

    private void RemoveStatFromInventory(StatSO statSO)
    {
        if (statSO == null)
        {
            if (debug) Debug.Log("StatSO is null, remotion will be ignored");
            return;
        }

        if (!statsInventory.Contains(statSO))
        {
            if (debug) Debug.Log($"StatSO with name {statSO.inventoryObjectName} is not on inventory, remotion will be ignored");
            return;
        }

        statsInventory.Remove(statSO);

        OnStatRemovedFromInventory?.Invoke(this, new OnStatEventArgs { statSO = statSO });
    }
}
