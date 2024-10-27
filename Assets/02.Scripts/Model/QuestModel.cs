using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThreeRabbitPackage;
using System.Linq;
using AlphabetNumber;
using UniRx;

public class QuestModel
{
    public class QuestItemModel
    {
        public ReactiveProperty<int> elpasedTime = new(0);
        public ReactiveProperty<int> level = new(0);
    }

    public List<QuestItemModel> questItemList = new();


    public void Init()
    {
        QuestTableList.Init(TRScriptableManager.Instance.GoogleSheet["Quest"]);

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
