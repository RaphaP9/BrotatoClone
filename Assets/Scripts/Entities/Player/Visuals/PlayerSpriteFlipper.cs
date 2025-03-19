using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteFlipper : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerDash playerDash;
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Components - Filled By Character Visual Instantiator")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool facingRight = true;

    private void OnEnable()
    {
        PlayerDash.OnPlayerDash += PlayerDash_OnPlayerDash;
    }

    private void OnDisable()
    {
        PlayerDash.OnPlayerDash -= PlayerDash_OnPlayerDash;
    }

    private void Update()
    {
        HandleFacingDueToMovement();
    }

    private void HandleFacingDueToMovement()
    {
        if (!playerHealth.IsAlive()) return;
        if (playerDash.IsDashing) return;

        if (playerMovement.DirectionInput.x> 0f)
        {
            CheckFaceRight();
        }

        if (playerMovement.DirectionInput.x < 0f)
        {
            CheckFaceLeft();
        }
    }

    private void CheckFaceRight()
    {
        if (facingRight) return;

        LookRight();
        facingRight = true;
    }

    private void CheckFaceLeft()
    {
        if (!facingRight) return;

        LookLeft();
        facingRight = false;
    }

    private void LookRight()
    {
        spriteRenderer.flipX = false;
    }

    private void LookLeft()
    {
        spriteRenderer.flipX = true;
    }

    private void PlayerDash_OnPlayerDash(object sender, PlayerDash.OnPlayerDashEventArgs e)
    {
        if(e.dashDirection.x > 0f)
        {
            CheckFaceRight();
        }

        if(e.dashDirection.x < 0f)
        {
            CheckFaceLeft();
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void SetSpriteRenderer(SpriteRenderer spriteRenderer) => this.spriteRenderer = spriteRenderer;
}
