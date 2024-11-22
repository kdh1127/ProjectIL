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
				hp.Value = GetHp(table);
				maxHp = GetHp(table);
				reward.Add(ECurrencyType.GOLD, BigInteger.Parse(table.Gold));
				break;
			case EMonsterType.BOSS:
				hp.Value = GetHp(table) * 5;
				maxHp = GetHp(table) * 5;
				reward.Add(ECurrencyType.GOLD, BigInteger.Parse(table.Gold));
				reward.Add(ECurrencyType.KEY, 2);
				break;
			case EMonsterType.TEN_BOSS:
				hp.Value = GetHp(table) * 10;
				maxHp = GetHp(table) * 10;
				reward.Add(ECurrencyType.GOLD, BigInteger.Parse(table.Gold));
				reward.Add(ECurrencyType.KEY, 5);
				reward.Add(ECurrencyType.DIA, 3);
				break;
			case EMonsterType.HUNDRED_BOSS:
				hp.Value = GetHp(table) * 50;
				maxHp = GetHp(table) * 50;
				reward.Add(ECurrencyType.GOLD, BigInteger.Parse(table.Gold));
				reward.Add(ECurrencyType.KEY, 10);
				reward.Add(ECurrencyType.DIA, 20);
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

	public Dictionary<ECurrencyType, BigInteger> GetReward()
	{
		return reward;
	}

	private BigInteger GetHp(StageTable table)
	{
		var rate = table.HpIncreasePer / 100;
		var baseHp = 500;
		var stageNo = StageManager.Instance.CurStage.Value;
		var increseHp = baseHp * rate * stageNo;
		var hp = baseHp + increseHp;
		return hp;
	}
}
