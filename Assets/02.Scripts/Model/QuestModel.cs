using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThreeRabbitPackage;
using System.Linq;
using AlphabetNumber;

public class QuestModel
{
    public class QuestItem
    {
        public int elpasedTime;
        public int level;

        public QuestItem()
        {
            elpasedTime = 0;
            level = 0;
        }
    }
    public List<QuestItem> questItemList = new();

    public void Init()
    {
        QuestTableList.Init(TRScriptableManager.Instance.GoogleSheet["Quest"]);

        for (int i = 0; i < QuestTableList.Get().Count; i++)
        {
            questItemList.Add(new QuestItem());
        }
    }

    public void Save()
    {
        var questTableList = QuestTableList.Get();

        for(int i = 0; i < questTableList.Count; i++)
        {
            PlayerPrefs.SetInt(questTableList[i].Name, questItemList[i].elpasedTime);
            PlayerPrefs.SetInt(questTableList[i].Name, questItemList[i].level);
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
