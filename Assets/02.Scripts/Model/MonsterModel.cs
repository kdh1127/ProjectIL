using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class MonsterModel : MonoBehaviour
{
    public ReactiveProperty<BigInteger> hp = new();
	public List<Reward> rewardList = new();
	public Subject<CommonClass.AttackInfo> AttackInfoSubject = new();
    public Subject<Unit> DeathSubject = new();
	public BigInteger maxHp;

    public void Init(StageTable table, EnumList.EMonsterType monsterType)
	{
		switch (monsterType)
		{
			case EnumList.EMonsterType.NORMAL:
				hp.Value = table.StageNo + table.Hp;
				maxHp = hp.Value;
				rewardList.Add(new Reward {amount = table.Gold, type = EnumList.ERewardType.GOLD });
				break;
			case EnumList.EMonsterType.BOSS:
				hp.Value = table.StageNo + table.Hp * 2;
				maxHp = hp.Value;
				rewardList.Add(new Reward { amount = table.Gold, type = EnumList.ERewardType.GOLD });
				rewardList.Add(new Reward { amount = table.Crystal, type = EnumList.ERewardType.DIA });
				rewardList.Add(new Reward { amount = table.Key, type = EnumList.ERewardType.KEY });
				break;
		}

	}
	public void TakeDamage(CommonClass.AttackInfo attackInfo)
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
		// 나중에 리팩토링 하자
		rewardList.ForEach(reward =>
		{
			switch (reward.type)
			{
				case EnumList.ERewardType.GOLD:
					CurrencyManager.Instance.AddGold(reward.amount);
					break;
				case EnumList.ERewardType.DIA:
					CurrencyManager.Instance.AddDia(reward.amount);
					break;
				case EnumList.ERewardType.KEY:
					CurrencyManager.Instance.Addkey(reward.amount);
					break;
			}
		});
	}
}
