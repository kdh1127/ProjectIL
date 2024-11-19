using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UniRx;
using UnityEngine;

public class TreasureItemModel
{
    public ReactiveProperty<int> level = new(0);
    private readonly TreasureTable table;

    public TreasureItemModel(TreasureTable table)
    {
        this.table = table;
    }

    UserDataManager.CharacterData character => UserDataManager.Instance.characterData;


    public void Upgrade()
    {
        var type = table.IncreaseType.ToEnum<EIncreaseType>();
        switch(type)
        {
            case EIncreaseType.EnemyGold:
                character.TreasureEnemyGoldPer = GetIncreaseValue();
                break;
            case EIncreaseType.QuestGold:
                character.TreasureQuestGoldPer = GetIncreaseValue();
                break;
            case EIncreaseType.CriticalDamage:
                character.TreasurecriticalDamagePer = GetIncreaseValue();
                break;
            case EIncreaseType.Damage:
                character.TreasureDamagePer = GetIncreaseValue();
                break;
            case EIncreaseType.ExtraDamage:
                character.TreasureExtraDamage = GetIncreaseValue();
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
    public List<TreasureItemModel> treasureItemList = new();

    public void Init(List<TreasureTable> tableList)
    {
        for (int i = 0; i < tableList.Count; i++)
        {
            treasureItemList.Add(new TreasureItemModel(tableList[i]));
        }
    }
}