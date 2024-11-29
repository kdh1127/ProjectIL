using System.Numerics;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class MonsterModel
{
    public ReactiveProperty<BigInteger> hp = new();
	private Dictionary<ECurrencyType,BigInteger> reward = new();
	public Subject<AttackInfo> AttackInfoSubject = new();
    public Subject<Unit> DeathSubject = new();
	public BigInteger maxHp;


	public void Init(StageTable table, EMonsterType monsterType)
	{

		var baseHp = StageManager.Instance.stageBaseHp;
		var baseGold = MonsterManager.Instance.CalcReward(table.Gold.ToBigInt());


		var curStage = StageManager.Instance.CurStage.Value;
		var stageIncreasePer = StageManager.Instance.GetCurStageTable().HpIncreasePer / 100f + 1;
		var BossCurPer = 5 * Mathf.Pow(stageIncreasePer, curStage);
		var factor = BossCurPer * 100;


		BigInteger bossHP = 570 * (int)factor / 100;
		if (curStage == 0)
		{
			bossHP = 570 * 5;
		}

		switch (monsterType)
		{
			case EMonsterType.NORMAL:
				hp.Value = baseHp;
				maxHp = baseHp;
				reward.Add(ECurrencyType.GOLD, baseGold);
				break;
			case EMonsterType.BOSS:
				hp.Value = bossHP;
				maxHp = bossHP;
				reward.Add(ECurrencyType.GOLD, baseGold * 5);
				reward.Add(ECurrencyType.KEY, 2);
				break;
			case EMonsterType.TEN_BOSS:
				hp.Value = bossHP * 10;
				maxHp = bossHP * 10;
				reward.Add(ECurrencyType.GOLD, baseGold * 10);
				reward.Add(ECurrencyType.KEY, 5);
				reward.Add(ECurrencyType.DIA, 3);
				break;
			case EMonsterType.HUNDRED_BOSS:
				hp.Value = bossHP * 50;
				maxHp = bossHP * 50;
				reward.Add(ECurrencyType.GOLD, baseGold * 50);
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
}
