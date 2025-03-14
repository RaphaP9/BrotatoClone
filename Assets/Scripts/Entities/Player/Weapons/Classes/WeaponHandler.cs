using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected WeaponSO weaponSO;

    [Header("Settings")]
    [SerializeField] protected LayerMask enemyLayerMask;
    public WeaponSO WeaponSO => weaponSO;

    protected bool InputAttack => AttackInput.Instance.GetAttackDown();

    protected int GetWeaponModifiedRegularDamage() => GeneralGameplayUtilities.GetWeaponModifiedDamage(weaponSO.regularDamage, AttackDamageMultiplierStatManager.Instance.AttackDamageMultiplierStat);
    protected int GetWeaponModifiedBleedDamage() => GeneralGameplayUtilities.GetWeaponModifiedDamage(weaponSO.bleedDamage, AttackDamageMultiplierStatManager.Instance.AttackDamageMultiplierStat);
    protected float GetWeaponModifiedCritChance() => GeneralGameplayUtilities.GetWeaponModifiedCritChance(weaponSO.critChance, AttackCritChanceStatManager.Instance.AttackCritChanceStat);
    protected float GetWeaponModifiedCritDamageMultiplier() => GeneralGameplayUtilities.GetWeaponModifiedCritDamageMultiplier(weaponSO.critDamageMultiplier, AttackCritDamageMultiplierStatManager.Instance.AttackCritDamageMultiplierStat);
}
