using System.Numerics;
using UnityEngine;

public class CharacterModel
{
    UserDataManager.CharacterData character => UserDataManager.Instance.characterData;
    public BigInteger BaseAttackDamage => (character.WeaponDamage
                                          + TreasureDamage
                                          + TreasureExtraDamage) * character.SkinStatDictionary[ESkinIncreaseType.Damage];

    /// <summary>
    /// �⺻ ���ݷ� * (�⺻ ũ��Ƽ�� ������ + (���� ũ��Ƽ�� ���� �ۼ�Ʈ�� / 100))
    /// </summary>
    public BigInteger CriticalAttackDamage => (BaseAttackDamage * character.CriticalDamage) + TreasureCriticalDamage * character.SkinStatDictionary[ESkinIncreaseType.CriticalDamage];

    /// <summary>
    /// ���� ������ + ������ ���� ������ ���� �ۼ�Ʈ / 100
    /// </summary>
    public BigInteger TreasureDamage => character.WeaponDamage * character.TreasureDamagePer / 100;

    public BigInteger TreasureExtraDamage => (character.WeaponDamage + TreasureDamage) * character.TreasureExtraDamage / 100;

    public BigInteger TreasureCriticalDamage => BaseAttackDamage * character.CriticalDamage * character.TreasureCriticalDamagePer / 100;

    public AttackInfo Attack()
    {
        bool isCritical = Random.Range(0, 101) < UserDataManager.Instance.characterData.CriticalChance;
        var damage = isCritical ? CriticalAttackDamage : BaseAttackDamage;

        return new AttackInfo(damage, isCritical);
    }
}
