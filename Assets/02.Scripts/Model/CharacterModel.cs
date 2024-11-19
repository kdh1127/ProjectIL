using System.Numerics;
using UnityEngine;

public class CharacterModel
{
    UserDataManager.CharacterData character => UserDataManager.Instance.characterData;
    public BigInteger BaseAttackDamage => character.WeaponDamage
                                          + TreasureDamageRate;

    /// <summary>
    /// 기본 공격력 * (기본 크리티컬 데미지 + (보물 크리티컬 증가 퍼센트값 / 100))
    /// </summary>
    public BigInteger CriticalAttackDamage => BaseAttackDamage * (UserDataManager.Instance.characterData.CriticalDamage + (character.TreasurecriticalDamagePer / 100));

    /// <summary>
    /// 무기 데미지 + 보물로 인한 데미지 증가 퍼센트 / 100
    /// </summary>
    public BigInteger TreasureDamageRate => character.WeaponDamage * character.TreasureDamagePer / 100;





    public AttackInfo Attack()
    {
        bool isCritical = Random.Range(0, 101) < UserDataManager.Instance.characterData.CriticalChance;
        var damage = isCritical ? CriticalAttackDamage : BaseAttackDamage;

        return new AttackInfo(damage, isCritical);
    }
}
