using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogueInput 
{
    public bool CanProcessInput();
    public bool GetSkipDown();
}
