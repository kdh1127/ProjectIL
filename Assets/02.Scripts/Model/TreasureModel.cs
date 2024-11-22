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
                if (key.Subtract(cost))
                {
                    level.Value++;
                    characterData.TreasureEnemyGoldPer = GetIncreaseValue();
                }
                break;
            case EIncreaseType.QuestGold:
                if (key.Subtract(cost))
                {
                    level.Value++;
                    characterData.TreasureQuestGoldPer = GetIncreaseValue();
                }
                break;
            case EIncreaseType.CriticalDamage:
                if (key.Subtract(cost))
                {
                    level.Value++;
                    characterData.TreasureCriticalDamagePer = GetIncreaseValue();
                }
                break;
            case EIncreaseType.Damage:
                if (key.Subtract(cost))
                {
                    level.Value++;
                    characterData.TreasureDamagePer = GetIncreaseValue();
                }
                break;
            case EIncreaseType.ExtraDamage:
                if (key.Subtract(cost))
                {
                    level.Value++;
                    characterData.TreasureExtraDamage = GetIncreaseValue();
                }
                break;
        }
    }

    public BigInteger GetIncreaseValue()
    {
        return level.Value * table.Increase.ToBigInt();
    }
}

public class TreasureModel
{
    public readonly List<TreasureItemModel> treasureItemList = new();
    private readonly CurrencyModel.Key key;


    [Inject]
    public TreasureModel(List<TreasureItemModel> treasureItemList, CurrencyModel.Key key)
    {
        this.treasureItemList = treasureItemList;
        this.key = key;
    }
    
    public void Init(List<TreasureTable> tableList)
    {
        for (int i = 0; i < tableList.Count; i++)
        {
            treasureItemList.Add(new TreasureItemModel(tableList[i], key));
        }
    }
}