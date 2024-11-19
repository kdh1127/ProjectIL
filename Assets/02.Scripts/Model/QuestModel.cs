using System.Numerics;
using System.Collections.Generic;
using UniRx;

public class QuestItemModel
{
    public ReactiveProperty<int> elpasedTime = new(0);
    public ReactiveProperty<int> level = new(0);
    private readonly QuestTable table;

    public bool IsOn => level.Value > 0;

    public QuestItemModel(QuestTable table) 
    {
        this.table = table; 
    }

    public void IncreaseLevel()
    {
        var missionData = UserDataManager.Instance.missiondata;
        var gold = CurrencyManager<Gold>.GetCurrency(ECurrencyType.GOLD);
        var cost = table.Cost.ToBigInt();

        level.Value++;
        gold.Sub(cost);
        missionData.UpdateQuestUpgradeData(table.QuestNo, currentValue => currentValue = level.Value);
    }

    public BigInteger GetReward(BigInteger increaseValue)
    {
        return level.Value > 0 ? (level.Value * increaseValue) : increaseValue;
    }

    public BigInteger GetReward()
    {
        var increaseValue = table.Increase.ToBigInt();
        return level.Value > 0 ? (level.Value * increaseValue) : increaseValue;
    }

    public void Progress(int endTime)
    {
        var isComplete = elpasedTime.Value >= endTime;
        var gold = CurrencyManager<Gold>.GetCurrency(ECurrencyType.GOLD);
        var missionData = UserDataManager.Instance.missiondata;
        var reward = GetReward();

        if (isComplete)
        {
            elpasedTime.Value = 0;
            gold.Add(reward);
            missionData.UpdateQuestClearData(table.QuestNo, currentValue => currentValue + 1);
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
            questItemList.Add(new QuestItemModel(questTableList[i]));
        }
    }

    public void Save()
    {
        DataUtility.Save("QuestModel", this);
    }

    public void Load()
    {
        var data = DataUtility.Load<QuestModel>("QuestModel");

        // TODO: Check Data
        if (data == null) return;

        questItemList.Clear();
        data.questItemList.ForEach(questItem =>
        {
            questItemList.Add(questItem);
        });
    }
}
