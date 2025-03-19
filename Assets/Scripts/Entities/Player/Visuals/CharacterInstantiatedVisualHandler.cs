using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInstantiatedVisualHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform renderingRefference;

    public Animator Animator => animator;
    public SpriteRenderer SpriteRenderer => spriteRenderer;
    public Transform RenderingRefference => renderingRefference;
}
