using System.Linq;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class MonsterModel
{
    public ReactiveProperty<BigInteger> hp = new();
	private Dictionary<ECurrencyType,BigInteger> reward = new();
	public Subject<AttackInfo> AttackInfoSubject = new();
    public Subject<Unit> DeathSubject = new();
	public BigInteger maxHp;

    public void Init(StageTable table, EMonsterType monsterType)
	{
		switch (monsterType)
		{
			case EMonsterType.NORMAL:
				hp.Value = table.StageNo + BigInteger.Parse(table.Hp);
				maxHp = hp.Value;
				reward.Add(ECurrencyType.GOLD, BigInteger.Parse(table.Gold));
				break;
			case EMonsterType.BOSS:
				hp.Value = table.StageNo + BigInteger.Parse(table.Hp) * 2;
				maxHp = hp.Value;
				reward.Add(ECurrencyType.GOLD, BigInteger.Parse(table.Gold));
				reward.Add(ECurrencyType.DIA, BigInteger.Parse(table.Dia));
				reward.Add(ECurrencyType.KEY, BigInteger.Parse(table.Key));
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
			CurrencyManager<IRCurrencyBase>.GetCurrency(reward.Key).Add(reward.Value);
		});
	}
}
