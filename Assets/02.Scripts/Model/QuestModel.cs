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
    private UserDataManager.MissionData MissionData => UserDataManager.Instance.missionData;
    private UserDataManager.CharacterData CharacterData => UserDataManager.Instance.characterData;
    private UserDataManager.QuestData QuestData => UserDataManager.Instance.questData;

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
            QuestData.questDict[table.QuestNo].level = m_level.Value;
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
            var factor = reward * CharacterData.TreasureQuestGoldPer;
            var totalReward = reward + (factor / 100);
            var result = totalReward == 0 ? reward : totalReward;
            gold.Add(result);
            MissionData.UpdateQuestClearData(table.QuestNo, currentValue => currentValue + 1);
        }
        else
        {
            m_elpasedTime.Value++;
        }

        QuestData.questDict[table.QuestNo].elpasedTime = m_elpasedTime.Value;
    }

    public void Reset()
    {
        m_elpasedTime.Value = 0;
        m_level.Value = 0;
    }
}

public class QuestModel
{
    public List<QuestItemModel> questItemList = new();
    public Subject<Unit> InitSubject = new();

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
        var questData = UserDataManager.Instance.questData.questDict;
        questItemList.Clear();
        questTableList.ForEach(table =>
        {
            var questItem = new QuestItemModel(table, gold);
            questItem.m_level.Value = questData[table.QuestNo].level;
            questItem.m_elpasedTime.Value = questData[table.QuestNo].elpasedTime;
            questItemList.Add(questItem);
        });
    }

    public void Reset()
    {
        questItemList.ForEach(questItem => questItem.Reset());
    }
}
