using System.Linq;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class MonsterModel
{
    public ReactiveProperty<BigInteger> hp = new();
	private Dictionary<EnumList.ECurrencyType,BigInteger> reward = new();
	public Subject<AttackInfo> AttackInfoSubject = new();
    public Subject<Unit> DeathSubject = new();
	public BigInteger maxHp;

    public void Init(StageTable table, EnumList.EMonsterType monsterType)
	{
		switch (monsterType)
		{
			case EnumList.EMonsterType.NORMAL:
				hp.Value = table.StageNo + BigInteger.Parse(table.Hp);
				maxHp = hp.Value;
				reward.Add(EnumList.ECurrencyType.GOLD, BigInteger.Parse(table.Gold));
				break;
			case EnumList.EMonsterType.BOSS:
				hp.Value = table.StageNo + BigInteger.Parse(table.Hp) * 2;
				maxHp = hp.Value;
				reward.Add(EnumList.ECurrencyType.GOLD, BigInteger.Parse(table.Gold));
				reward.Add(EnumList.ECurrencyType.DIA, BigInteger.Parse(table.Dia));
				reward.Add(EnumList.ECurrencyType.KEY, BigInteger.Parse(table.Key));
				break;
		}

	}
	public void TakeDamage(AttackInfo attackInfo)
    {
        hp.Value -= attackInfo.damage;
		AttackInfoSubject.OnNext(attackInfo);
		if (hp.Value <= 0)
		{
			DeathSubject.OnNext(Unit.Default);
		}
    }

	public void AddReward()
	{
		reward.ToList().ForEach(reward =>
		{
			CurrencyManager.Instance.AddCurrency(reward.Key, reward.Value);
		});
	}
}
