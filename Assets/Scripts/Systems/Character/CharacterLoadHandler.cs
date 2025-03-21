using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLoadHandler : MonoBehaviour
{
    public static CharacterSO LoadedCharacter {  get; private set; }

    [Header("Components")]
    [SerializeField] private PlayerIdentifier playerIdentifier;

    [Header("Settings")]
    [SerializeField] private CharacterSO defaultCharacter;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private void Awake()
    {
        LoadCharacterFromSelectedCharacter();
    }

    private void LoadCharacterFromSelectedCharacter()
    {
        CharacterSO characterSO = CharacterSelectionHandler.SelectedCharacter;

        if(characterSO == null)
        {
            if (debug) Debug.Log("SelectedCharacter from CharacterSelectionHandler is null. Proceding to load default character.");
            characterSO = defaultCharacter;
        }

        SetLoadedCharacter(characterSO);
        playerIdentifier.SetCharacterSO(characterSO);
    }

    private void SetLoadedCharacter(CharacterSO characterSO) => LoadedCharacter = characterSO;

}
