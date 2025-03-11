using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementInput
{
    public bool CanProcessInput();
    public Vector2 GetMovementInputNormalized();
    public bool GetJumpDown();
    public bool GetJump();
    public bool GetDashDown();
}
