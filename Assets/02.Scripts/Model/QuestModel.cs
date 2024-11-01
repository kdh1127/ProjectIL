using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThreeRabbitPackage;
using System.Linq;
using AlphabetNumber;
using UniRx;

public class QuestItemModel
{
    public ReactiveProperty<int> elpasedTime = new(0);
    public ReactiveProperty<int> level = new(0);

    public bool IsOn => level.Value > 0;

    public void Upgrade(QuestTable table)
    {
        if (CurrencyManager.Instance.AddGold(-table.Cost))
        {
            level.Value++;
        }
    }

    public int GetReward(QuestTable table)
    {
        return level.Value > 0 ? level.Value * table.Increase : table.Increase;
    }

    public void Progress(QuestTable table)
    {
        var reward = GetReward(table);

        if (elpasedTime.Value >= table.Time)
        {
            CurrencyManager.Instance.AddGold(reward);
            elpasedTime.Value = 0;
        }
        else
        {
            elpasedTime.Value++;
        }
    }
}

public class QuestModel
{
    public List<QuestItemModel> questItemList = new();


    public void Init()
    {
        var table = TRScriptableManager.Instance.GoogleSheet["Quest"];
        QuestTableList.Init(table);

        for (int i = 0; i < QuestTableList.Get().Count; i++)
        {
            questItemList.Add(new QuestItemModel());
        }
    }

    public void Save()
    {
        var questTableList = QuestTableList.Get();

        for(int i = 0; i < questTableList.Count; i++)
        {
            PlayerPrefs.SetInt(questTableList[i].Name, questItemList[i].elpasedTime.Value);
            PlayerPrefs.SetInt(questTableList[i].Name, questItemList[i].level.Value);
        }
    }

    public void Load()
    {
        QuestTableList.Get().ForEach(quest =>
        {
            PlayerPrefs.GetInt(quest.Name, 0);
        });
    }
}
