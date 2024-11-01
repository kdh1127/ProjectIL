using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CharacterModel
{
    public float moveSpeed = 1f;
    public float attackPerSecond = 1;
    public BigInteger weaponDamage = 5;
    public BigInteger criticalDamage = 2;
    public BigInteger criticalChance = 10;

    public BigInteger baseAttackDamage => weaponDamage;
    public BigInteger criticalAttackDamage => baseAttackDamage * criticalDamage;
            
    public AttackInfo Attack()
    {
        bool isCritical = Random.Range(0, 101) < criticalChance;
        var damage = isCritical ? criticalAttackDamage : baseAttackDamage;

        return new AttackInfo(damage, isCritical);
    }
}
