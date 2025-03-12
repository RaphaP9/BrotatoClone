using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAugmentSO", menuName = "ScriptableObjects/Augments/Augment")]
public abstract class AugmentSO : ScriptableObject
{
    public string augmentName;
    public AugmentRarity augmentRarity;
    public AugmentType augmentType;
}
