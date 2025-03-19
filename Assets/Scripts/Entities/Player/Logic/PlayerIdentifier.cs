using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdentifier : MonoBehaviour
{
    public static PlayerIdentifier Instance { get; private set; }

    [Header("Components - Filled By Character Load Handler")]
    [SerializeField] private CharacterSO characterSO;

    public CharacterSO CharacterSO => characterSO;

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

    public void SetCharacterSO(CharacterSO characterSO) => this.characterSO = characterSO;

}
