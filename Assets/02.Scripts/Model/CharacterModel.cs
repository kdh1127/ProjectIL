using System.Numerics;
using UnityEngine;

public class CharacterModel
{
    UserDataManager.CharacterData character => UserDataManager.Instance.characterData;
    public BigInteger BaseAttackDamage => (character.WeaponDamage
                                          + TreasureDamage
                                          + TreasureExtraDamage) + SkinDamage;

    /// <summary>
    /// �⺻ ���ݷ� * (�⺻ ũ��Ƽ�� ������ + (���� ũ��Ƽ�� ���� �ۼ�Ʈ�� / 100))
    /// </summary>
    public BigInteger CriticalAttackDamage => (BaseAttackDamage * character.CriticalDamage) + TreasureCriticalDamage + SkinCriticalDamage;

    /// <summary>
    /// ���� ������ + ������ ���� ������ ���� �ۼ�Ʈ / 100
    /// </summary>
    public BigInteger TreasureDamage => character.WeaponDamage * character.TreasureDamagePer / 100;

    public BigInteger TreasureExtraDamage => (character.WeaponDamage + TreasureDamage) * character.TreasureExtraDamage / 100;

    public BigInteger TreasureCriticalDamage => BaseAttackDamage * character.CriticalDamage * character.TreasureCriticalDamagePer / 100;

    public BigInteger SkinDamage => (character.WeaponDamage
                                          + TreasureDamage
                                          + TreasureExtraDamage) * character.SkinStatDictionary[ESkinIncreaseType.Damage] / 100;

    public BigInteger SkinCriticalDamage => (BaseAttackDamage * character.CriticalDamage + TreasureCriticalDamage)
                                            * character.SkinStatDictionary[ESkinIncreaseType.CriticalDamage] / 100;

    public BigInteger QuestGoldIncrease => (1 * character.TreasureQuestGoldPer) / 100 == 0? 1 : (1 * character.TreasureQuestGoldPer);


    /// <summary>
    /// �̵��ӵ� ���
    /// </summary>
    private float runSpeedFactor => character.MoveSpeed * (float)character.SkinStatDictionary[ESkinIncreaseType.RunSpeed] / 100f;
    public float RunSpeed => character.MoveSpeed + runSpeedFactor;

    /// <summary>
    /// ���ݼӵ� ���
    /// </summary>
    private float attackSpeedFactor => character.MoveSpeed * (float)character.SkinStatDictionary[ESkinIncreaseType.AttackSpeed] / 100f;
    public float AttackSpeed => character.AttackPerSecond + attackSpeedFactor;

    public AttackInfo Attack()
    {
        bool isCritical = Random.Range(0, 101) < UserDataManager.Instance.characterData.CriticalChance;
        var damage = isCritical ? CriticalAttackDamage : BaseAttackDamage;

        return new AttackInfo(damage, isCritical);
    }
}
