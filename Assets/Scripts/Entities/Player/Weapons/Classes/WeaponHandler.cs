using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected WeaponSO weaponSO;

    [Header("Debug")]
    [SerializeField] protected bool debug;
    [SerializeField] protected Color gizmosColor;

    public WeaponSO WeaponSO => weaponSO;

    protected bool SemiAutomaticInputAttack => AttackInput.Instance.GetAttackDown();
    protected bool AutomaticInputAttack => AttackInput.Instance.GetAttackHold();

    protected int GetWeaponModifiedRegularDamage() => GeneralGameplayUtilities.GetModifiedDamage(weaponSO.regularDamage, AttackDamageMultiplierStatManager.Instance.AttackDamageMultiplierStat);
    protected int GetWeaponModifiedBleedDamage() => GeneralGameplayUtilities.GetModifiedDamage(weaponSO.bleedDamage, AttackDamageMultiplierStatManager.Instance.AttackDamageMultiplierStat);
    protected float GetWeaponModifiedCritChance() => GeneralGameplayUtilities.GetModifiedCritChance(weaponSO.critChance, AttackCritChanceStatManager.Instance.AttackCritChanceStat);
    protected float GetWeaponModifiedCritDamageMultiplier() => GeneralGameplayUtilities.GetModifiedCritDamageMultiplier(weaponSO.critDamageMultiplier, AttackCritDamageMultiplierStatManager.Instance.AttackCritDamageMultiplierStat);

    protected bool GetAttackInput()
    {
        switch (weaponSO.fireType)
        {
            case FireType.SemiAutomatic:
            default:
                return SemiAutomaticInputAttack;
            case FireType.Automatic:
                return AutomaticInputAttack;
        }
    }
}
