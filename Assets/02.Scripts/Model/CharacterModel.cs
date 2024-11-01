using System.Numerics;
using UnityEngine;

public class CharacterModel
{
    public BigInteger BaseAttackDamage => UserDataManager.Instance.characterData.WeaponDamage;
    public BigInteger CriticalAttackDamage => BaseAttackDamage * UserDataManager.Instance.characterData.CriticalDamage;
            
    public AttackInfo Attack()
    {
        bool isCritical = Random.Range(0, 101) < UserDataManager.Instance.characterData.CriticalDamage;
        var damage = isCritical ? CriticalAttackDamage : BaseAttackDamage;

        return new AttackInfo(damage, isCritical);
    }
}
