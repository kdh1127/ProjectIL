using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UniRx;
using UnityEngine;
using Zenject;

public class TreasureItemModel
{
    public ReactiveProperty<int> level = new(0);
    private readonly TreasureTable table;
    private readonly CurrencyModel.Key key;

    public TreasureItemModel(TreasureTable table ,CurrencyModel.Key key)
    {
        this.table = table;
        this.key = key;
    }

    UserDataManager.CharacterData characterData => UserDataManager.Instance.characterData;


    public void Upgrade()
    {
        var type = table.IncreaseType.ToEnum<EIncreaseType>();
        var cost = table.TreasureCost.ToBigInt();

        switch(type)
        {
            case EIncreaseType.EnemyGold:
                if (key.Subtract(GetCost()))
                {
                    level.Value++;
                    characterData.TreasureEnemyGoldPer = GetIncreaseValue();
                }
                break;
            case EIncreaseType.QuestGold:
                if (key.Subtract(GetCost()))
                {
                    level.Value++;
                    characterData.TreasureQuestGoldPer = GetIncreaseValue();
                }
                break;
            case EIncreaseType.CriticalDamage:
                if (key.Subtract(GetCost()))
                {
                    level.Value++;
                    characterData.TreasureCriticalDamagePer = GetIncreaseValue();
                }
                break;
            case EIncreaseType.Damage:
                if (key.Subtract(GetCost()))
                {
                    level.Value++;
                    characterData.TreasureDamagePer = GetIncreaseValue();
                }
                break;
            case EIncreaseType.ExtraDamage:
                if (key.Subtract(GetCost()))
                {
                    level.Value++;
                    characterData.TreasureExtraDamage = GetIncreaseValue();
                }
                break;
        }

        var data = UserDataManager.Instance.treasureData.treasureList;
        data[table.TreasureNo].level = level.Value;
    }

    public BigInteger GetIncreaseValue()
    {
        return level.Value * table.Increase.ToBigInt();
    }

    public BigInteger GetCost()
	{
        var baseCost = table.TreasureCost.ToBigInt();
        return baseCost + (baseCost * level.Value * 12 / 100);
	}
}

public class TreasureModel
{
    public readonly List<TreasureTable> treasureTableList = new();
    private readonly CurrencyModel.Key key;

    public List<TreasureItemModel> treasureItemList = new();


    [Inject]
    public TreasureModel(List<TreasureTable> treasureTableList, CurrencyModel.Key key)
    {
        this.treasureTableList = treasureTableList;
        this.key = key;
    }
    
    public void Init()
    {
        var data = UserDataManager.Instance.treasureData.treasureList;
        treasureItemList.Clear();
        treasureTableList.ForEach(table =>
        {
            var treasureItem = new TreasureItemModel(table, key);
            treasureItem.level.Value = data[table.TreasureNo].level;
            treasureItemList.Add(treasureItem);
        });
    }
}