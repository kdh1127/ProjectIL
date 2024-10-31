using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CharacterModel : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float attackPerSecond = 1;
    public BigInteger weaponDamage = 0;
    public BigInteger criticalDamage = 2;
    public BigInteger criticalChance = 10;

    public BigInteger baseAttackDamage => weaponDamage;
    public BigInteger criticalAttackDamage => baseAttackDamage * criticalDamage;
            
    public BigInteger Attack()
    {
        var random = Random.Range(0, 101); // 0 ~ 100

        if (criticalChance >= random)
        {
            return criticalAttackDamage;
        }
        else
        {
            return baseAttackDamage;
        }
    }
}
