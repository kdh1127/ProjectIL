using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThreeRabbitPackage;

public class QuestModel
{
    public TRGoogleSheet questTable;
    public TRSpriteResources questImageResources;

    public void Init()
    {
        questTable = Resources.Load<TRGoogleSheet>("Table/QuestTable");
        questImageResources = Resources.Load<TRSpriteResources>("QuestImageResources");
        QuestTableList.Init(questTable);
    }
}
