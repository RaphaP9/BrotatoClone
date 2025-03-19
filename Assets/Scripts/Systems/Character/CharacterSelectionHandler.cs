using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionHandler : MonoBehaviour
{
    public static CharacterSO SelectedCharacter { get; private set; }

    private void SetSelectedCharacter(CharacterSO characterSO) => SelectedCharacter = characterSO;
}
