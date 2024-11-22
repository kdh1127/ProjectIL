using System.Numerics;
using System.Collections.Generic;
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
		var baseHp = StageManager.Instance.stageBaseHp;
		switch (monsterType)
		{
			case EMonsterType.NORMAL:
				hp.Value = baseHp;
				maxHp = baseHp;
				reward.Add(ECurrencyType.GOLD, BigInteger.Parse(table.Gold));
				break;
			case EMonsterType.BOSS:
				hp.Value = baseHp * 5;
				maxHp = baseHp * 5;
				reward.Add(ECurrencyType.GOLD, BigInteger.Parse(table.Gold));
				reward.Add(ECurrencyType.KEY, 2);
				break;
			case EMonsterType.TEN_BOSS:
				hp.Value = baseHp * 10;
				maxHp = baseHp * 10;
				reward.Add(ECurrencyType.GOLD, BigInteger.Parse(table.Gold));
				reward.Add(ECurrencyType.KEY, 5);
				reward.Add(ECurrencyType.DIA, 3);
				break;
			case EMonsterType.HUNDRED_BOSS:
				hp.Value = baseHp * 50;
				maxHp = baseHp * 50;
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
}
