using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsNameAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("Settings")]
    [SerializeField, Range (0f,10f)] private float startingOffset;
    [SerializeField, Range (2f,10f)] private float minMoveInterval;
    [SerializeField, Range (3f,15f)] private float maxMoveInterval;

    private const string MOVE_ANIMATION_NAME = "Move";

    private void Start()
    {
        StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        yield return new WaitForSeconds(startingOffset);

        while (true)
        {
            float interval = GeneralUtilities.GetRandomFloatBetween(minMoveInterval, maxMoveInterval);

            yield return new WaitForSeconds(interval);

            animator.Play(MOVE_ANIMATION_NAME);
        }
    }
}
