using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TreasureItemModel
{
    public ReactiveProperty<int> level = new(0);

    void Upgrade()
    {

    }

    void SetTreasureTotalIncrease()
    {

    }
}

public class TreasureModel
{
    public List<TreasureItemModel> treasureItemList = new();

    public void Init()
    {
        var table = TRScriptableManager.Instance.GetGoogleSheet("TreasureTable");
        TreasureTableList.Init(table);

        for (int i = 0; i < TreasureTableList.Get().Count; i++)
        {
            treasureItemList.Add(new TreasureItemModel());
        }
    }
}