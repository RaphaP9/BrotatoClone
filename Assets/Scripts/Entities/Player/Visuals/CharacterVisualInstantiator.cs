using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVisualInstantiator : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerAnimatorController playerAnimatorController;
    [SerializeField] private PlayerSpriteFlipper playerSpriteFlipper;
    [SerializeField] private EntitySortingOrderRenderingHandler entitySortingOrderRenderingHandler;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private void Start()
    {
        InstantiateCharacterVisual();
    }

    private void InstantiateCharacterVisual()
    {
        Transform characterVisual = Instantiate(PlayerIdentifier.Instance.CharacterSO.characterVisualTransform, transform);

        CharacterInstantiatedVisualHandler characterInstantiatedVisualHandler = characterVisual.GetComponent<CharacterInstantiatedVisualHandler>();

        if(characterInstantiatedVisualHandler == null )
        {
            if (debug) Debug.LogError("Instantiated Character Visual does not contain a CharacterInstantiatedVisualHandler. Throwing error.");
            return;
        }

        AssignScriptRefferences(characterInstantiatedVisualHandler);
    }

    private void AssignScriptRefferences(CharacterInstantiatedVisualHandler characterInstantiatedVisualHandler)
    {
        playerAnimatorController.SetAnimator(characterInstantiatedVisualHandler.Animator);

        playerSpriteFlipper.SetSpriteRenderer(characterInstantiatedVisualHandler.SpriteRenderer);

        entitySortingOrderRenderingHandler.SetSpriteRenderer(characterInstantiatedVisualHandler.SpriteRenderer);
        entitySortingOrderRenderingHandler.SetRenderingRefference(characterInstantiatedVisualHandler.RenderingRefference);
    }
}
