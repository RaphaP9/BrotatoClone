using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageDealer 
{
    public Color GetDamageColor();
    public string GetName();
    public string GetDescription();
    public Sprite GetSprite();

    public DamageDealerClassification GetDamageDealerClassification();
}
