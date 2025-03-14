using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpriteFlipper : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private WeaponAim weaponAim;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool facingRight = true;

    private void Update()
    {
        HandleFacingDueToAim();
    }

    private void HandleFacingDueToAim()
    {
        if (weaponAim.FacingRight)
        {
            CheckFaceRight();
        }

        if (!weaponAim.FacingRight)
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
        spriteRenderer.flipY = false;
    }

    private void LookLeft()
    {
        spriteRenderer.flipY = true;
    }
}
