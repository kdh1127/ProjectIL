using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CharacterModel
{
    public BigInteger BaseAttackDamage => UserDataManager.Instance.WeaponDamage;
    public BigInteger CriticalAttackDamage => BaseAttackDamage * UserDataManager.Instance.CriticalDamage;
            
    public AttackInfo Attack()
    {
        bool isCritical = Random.Range(0, 101) < UserDataManager.Instance.CriticalDamage;
        var damage = isCritical ? CriticalAttackDamage : BaseAttackDamage;

        return new AttackInfo(damage, isCritical);
    }
}
