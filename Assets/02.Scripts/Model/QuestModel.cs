using System.Numerics;
using System.Collections.Generic;
using UniRx;
using Zenject;
public class QuestItemModel
{
    public ReactiveProperty<int> m_elpasedTime = new(0);
    public ReactiveProperty<int> m_level = new(0);

    private readonly QuestTable table;
    private readonly CurrencyModel.Gold gold;

    public bool IsOn => m_level.Value > 0;
    private UserDataManager.MissionData MissionData => UserDataManager.Instance.missiondata;

    [Inject]
    public QuestItemModel(QuestTable table, CurrencyModel.Gold gold) 
    {
        this.table = table;
        this.gold = gold;
    }

    public void IncreaseLevel()
    {
        var cost = table.Cost.ToBigInt();
        
        if(gold.Subtract(cost))
		{
            m_level.Value++;
            MissionData.UpdateQuestUpgradeData(table.QuestNo, currentValue => currentValue = m_level.Value);
        }
    }

    public BigInteger GetReward()
    {
        var increaseValue = table.Increase.ToBigInt();
        return m_level.Value > 0 ? (m_level.Value * increaseValue) : increaseValue;
    }

    public void Progress(int endTime)
    {
        var isComplete = m_elpasedTime.Value >= endTime;
        var reward = GetReward();

        if (isComplete)
        {
            m_elpasedTime.Value = 0;
            gold.Add(reward);
            MissionData.UpdateQuestClearData(table.QuestNo, currentValue => currentValue + 1);
        }
        else
        {
            m_elpasedTime.Value++;
        }
    }
}

public class QuestModel
{
    public List<QuestItemModel> questItemList = new();

    private readonly List<QuestTable> questTableList;
    private readonly CurrencyModel.Gold gold;

    [Inject]
    public QuestModel(List<QuestTable> questTableList, CurrencyModel.Gold gold)
	{
        this.questTableList = questTableList;
        this.gold = gold;
    }

    public void Init()
    {
        questTableList.ForEach(table => questItemList.Add(new QuestItemModel(table, gold)));
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
        data.questItemList.ForEach(questItem => questItemList.Add(questItem));
    }
}
