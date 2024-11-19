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

    public void Init(List<TreasureTable> tableList)
    {
        for (int i = 0; i < tableList.Count; i++)
        {
            treasureItemList.Add(new TreasureItemModel());
        }
    }
}