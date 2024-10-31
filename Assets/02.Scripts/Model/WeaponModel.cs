using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Numerics;

public class WeaponItemModel
{
    public ReactiveProperty<int> level = new(0);
    public ReactiveProperty<BigInteger> totalAtk = new(0);

    public void Upgrade(WeaponTable table)
    {
        if (CurrencyManager.Instance.AddGold(-BigInteger.Parse(table.Cost)))
        {
            level.Value++;
        }
    }
}

public class WeaponModel
{
    public List<WeaponItemModel> weaponItemList = new();


    public void Init()
    {
        WeaponTableList.Init(TRScriptableManager.Instance.GoogleSheet["Weapon"]);

        for (int i = 0; i < WeaponTableList.Get().Count; i++)
        {
            weaponItemList.Add(new WeaponItemModel());
        }
    }


}
