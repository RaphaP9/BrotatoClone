using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackInput
{
    public bool CanProcessInput();
    public bool GetAttackDown();
    public bool GetAttackHold();
}
