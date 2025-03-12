using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AbilitiesInventoryManager : MonoBehaviour
{
    public static AbilitiesInventoryManager Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<AbilitySO> abilitiesInventory;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public List<AbilitySO> AbilitiesInventory => abilitiesInventory;

    public static event EventHandler<OnAbilityEventArgs> OnAbilityAddedToInventory;
    public static event EventHandler<OnAbilityEventArgs> OnAbilityRemovedFromInventory;

    public class OnAbilityEventArgs : EventArgs
    {
        public AbilitySO abilitySO;
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
            Debug.LogWarning("There is more than one AbilitiesInventoryManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void AddAbilityToInventory(AbilitySO abilitySO)
    {
        if (abilitySO == null)
        {
            if (debug) Debug.Log("AbilitySO is null, addition will be ignored");
            return;
        }

        if (abilitiesInventory.Contains(abilitySO))
        {
            if (debug) Debug.Log($"AbilitySO with name {abilitySO.inventoryObjectName} is already on inventory, addition will be ignored");
            return;
        }

        abilitiesInventory.Add(abilitySO);

        OnAbilityAddedToInventory?.Invoke(this, new OnAbilityEventArgs { abilitySO = abilitySO });
    }

    private void RemoveAbilityFromInventory(AbilitySO abilitySO)
    {
        if (abilitySO == null)
        {
            if (debug) Debug.Log("AbilitySO is null, remotion will be ignored");
            return;
        }

        if (!abilitiesInventory.Contains(abilitySO))
        {
            if (debug) Debug.Log($"AbilitySO with name {abilitySO.inventoryObjectName} is not on inventory, remotion will be ignored");
            return;
        }

        abilitiesInventory.Remove(abilitySO);

        OnAbilityRemovedFromInventory?.Invoke(this, new OnAbilityEventArgs { abilitySO = abilitySO });
    }
}