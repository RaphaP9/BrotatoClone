using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AbilitiesInventoryManager : MonoBehaviour
{
    public static AbilitiesInventoryManager Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<AbilityInventoryIdentified> abilitiesInventory;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public List<AbilityInventoryIdentified> AbilitiesInventory => abilitiesInventory;

    public static event EventHandler<OnAbilitiesEventArgs> OnAbilitiesInventoryInitialized;
    public static event EventHandler<OnAbilityEventArgs> OnAbilityAddedToInventory;
    public static event EventHandler<OnAbilityEventArgs> OnAbilityRemovedFromInventory;

    public class OnAbilitiesEventArgs : EventArgs
    {
        public List<AbilityInventoryIdentified> abilities;
    }

    public class OnAbilityEventArgs : EventArgs
    {
        public AbilityInventoryIdentified ability;
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

    private void Start()
    {
        SetAbilitiesInventoryFromCharacter();
        InitializeAbilitiesInventory();
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

    private void InitializeAbilitiesInventory()
    {
        OnAbilitiesInventoryInitialized?.Invoke(this, new OnAbilitiesEventArgs { abilities = abilitiesInventory });
    }

    private void AddAbilityToInventory(AbilitySO abilitySO)
    {
        if (abilitySO == null)
        {
            if (debug) Debug.Log("AbilitySO is null, addition will be ignored");
            return;
        }

        string abilityGUID = GeneralDataUtilities.GenerateGUID();

        AbilityInventoryIdentified abilityToAdd = new AbilityInventoryIdentified { GUID = abilityGUID, abilitySO = abilitySO };

        abilitiesInventory.Add(abilityToAdd);

        OnAbilityAddedToInventory?.Invoke(this, new OnAbilityEventArgs { ability = abilityToAdd });
    }

    private void RemoveAbilityFromInventoryByAbilitySO(AbilitySO abilitySO)
    {
        if (abilitySO == null)
        {
            if (debug) Debug.Log("AbilitySO is null, remotion will be ignored");
            return;
        }

        AbilityInventoryIdentified abilityIdentified = FindAbilityByAbilitySO(abilitySO);

        if (abilitiesInventory == null)
        {
            if (debug) Debug.Log("Could not find ability by AbilitySO");
            return;
        }

        abilitiesInventory.Remove(abilityIdentified);

        OnAbilityRemovedFromInventory?.Invoke(this, new OnAbilityEventArgs { ability = abilityIdentified });
    }

    private void RemoveAbilityFromInventoryByGUID(string GUID)
    {
        AbilityInventoryIdentified abilityIdentified = FindAbilityByGUID(GUID);

        if (abilitiesInventory == null)
        {
            if (debug) Debug.Log("Could not find ability by GUID");
            return;
        }

        abilitiesInventory.Remove(abilityIdentified);

        OnAbilityRemovedFromInventory?.Invoke(this, new OnAbilityEventArgs { ability = abilityIdentified });
    }

    private AbilityInventoryIdentified FindAbilityByAbilitySO(AbilitySO abilitySO)
    {
        foreach (AbilityInventoryIdentified ability in abilitiesInventory)
        {
            if (ability.abilitySO == abilitySO) return ability;
        }

        if (debug) Debug.Log($"Ability with AbilitySO with ID {abilitySO.id} could not be found. Proceding to return null");
        return null;
    }

    private AbilityInventoryIdentified FindAbilityByGUID(string GUID)
    {
        foreach (AbilityInventoryIdentified ability in abilitiesInventory)
        {
            if (ability.GUID == GUID) return ability;
        }

        if (debug) Debug.Log($"Ability with GUID {GUID} could not be found. Proceding to return null");
        return null;
    }


    private void SetAbilitiesInventoryFromCharacter()
    {
        ClearAbilitiesInventory();
        AddAbilitiesToInventory(PlayerIdentifier.Instance.CharacterSO.startingAbilities);
    }

    private void AddAbilitiesToInventory(List<AbilitySO> abilitiesSOs)
    {
        foreach (AbilitySO abilitySOs in abilitiesSOs)
        {
            AddAbilityToInventory(abilitySOs);
        }
    }

    private void ClearAbilitiesInventory() => abilitiesInventory.Clear();

    public bool AbilitiesInventoryFull() => false;

    public bool AbilityInInventoryByAbilitySO(AbilitySO abilitySO)
    {
        foreach (AbilityInventoryIdentified ablity in abilitiesInventory)
        {
            if (ablity.abilitySO == abilitySO) return true;
        }

        return false;
    }
}

[System.Serializable]
public class AbilityInventoryIdentified
{
    public string GUID;
    public AbilitySO abilitySO;
}