using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GeneralDataUtilities
{
    public static string GenerateGUID()
    {
        string generatedGUID = Guid.NewGuid().ToString();
        return generatedGUID;
    }
}
