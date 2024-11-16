using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThreeRabbitPackage;
using System.Linq;
using UniRx;
using Zenject;

public class QuestItemModel
{
    public ReactiveProperty<int> elpasedTime = new(0);
    public ReactiveProperty<int> level = new(0);
    public Subject<Unit> questClearSubject = new();

    public bool IsOn => level.Value > 0;

    public void IncreaseLevel()
    {
        level.Value++;
    }

    public BigInteger GetReward(BigInteger increaseValue)
    {
        return level.Value > 0 ? (level.Value * increaseValue) : increaseValue;
    }

    public void Progress(int endTime)
    {
        var isComplete = elpasedTime.Value >= endTime;

        if (isComplete)
        {
            elpasedTime.Value = 0;
            questClearSubject.OnNext(Unit.Default);
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

    public void Init(List<QuestTable> questTableList)
    {
        for (int i = 0; i < questTableList.Count; i++)
        {
            questItemList.Add(new QuestItemModel());
        }
    }

    public void Save()
    {
        DataUtility.Save("QuestModel", this);
    }

    public void Load()
    {
        var data = DataUtility.Load<QuestModel>("QuestModel");

        data.questItemList.ForEach(questItem =>
        {
            questItemList.Add(questItem);
        });
    }
}
