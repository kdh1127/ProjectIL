using System.Numerics;
using UnityEngine;

public class CharacterModel
{
    UserDataManager.CharacterData character => UserDataManager.Instance.characterData;
    public BigInteger BaseAttackDamage => character.WeaponDamage
                                          + TreasureDamageRate;

    /// <summary>
    /// �⺻ ���ݷ� * (�⺻ ũ��Ƽ�� ������ + (���� ũ��Ƽ�� ���� �ۼ�Ʈ�� / 100))
    /// </summary>
    public BigInteger CriticalAttackDamage => BaseAttackDamage * (UserDataManager.Instance.characterData.CriticalDamage + (character.TreasurecriticalDamagePer / 100));

    /// <summary>
    /// ���� ������ + ������ ���� ������ ���� �ۼ�Ʈ / 100
    /// </summary>
    public BigInteger TreasureDamageRate => character.WeaponDamage * character.TreasureDamagePer / 100;





    public AttackInfo Attack()
    {
        bool isCritical = Random.Range(0, 101) < UserDataManager.Instance.characterData.CriticalChance;
        var damage = isCritical ? CriticalAttackDamage : BaseAttackDamage;

        return new AttackInfo(damage, isCritical);
    }
}
