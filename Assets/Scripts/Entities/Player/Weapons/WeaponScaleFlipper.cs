using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScaleFlipper : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private WeaponAim weaponAim;
    [SerializeField] private Transform spriteHolder;

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
        spriteHolder.localScale = new Vector3(1, 1, 1);
    }

    private void LookLeft()
    {
        spriteHolder.localScale = new Vector3(1, -1, 1);
    }
}
