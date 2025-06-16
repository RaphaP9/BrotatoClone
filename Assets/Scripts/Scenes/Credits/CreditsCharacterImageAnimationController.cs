using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsCharacterImageAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("Settings")]
    [SerializeField, Range(2f, 10f)] private float minBlinkInterval;
    [SerializeField, Range(3f, 15f)] private float maxBlinkInterval;

    private const string BLINK_ANIMATION_NAME = "Blink";

    private void Start()
    {
        StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        while (true)
        {
            float interval = GeneralUtilities.GetRandomFloatBetween(minBlinkInterval, maxBlinkInterval);

            yield return new WaitForSeconds(interval);

            animator.Play(BLINK_ANIMATION_NAME);
        }
    }
}
