using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Numerics;
using System.Linq;

public class WeaponItemModel
{
    public ReactiveProperty<int> level = new(0);
    public bool IsMaxLevel => level.Value == 5;
    public bool isEquiped = false;
    public bool isUnLock = false;
    public Subject<int> UpdateSubject = new();

    public void Upgrade(WeaponTable table)
    {
        var userWeaponUpgradeData = UserDataManager.Instance.missiondata.WeaponUpgradeData;
        if (CurrencyManager.Instance.AddCurrency(EnumList.ECurrencyType.GOLD,-BigInteger.Parse(table.Cost)))
        {
            level.Value++;
            UpdateSubject.OnNext(table.WeaponNo);
            if (userWeaponUpgradeData.Keys.Contains(table.WeaponNo)) 
            {
                userWeaponUpgradeData[table.WeaponNo] = level.Value;
            }
        }
    }
}

public class WeaponModel
{
    public List<WeaponItemModel> weaponItemList = new();

    public void Init()
    {
        WeaponTableList.Init(TRScriptableManager.Instance.GetGoogleSheet("WeaponTable"));

        for (int i = 0; i < WeaponTableList.Get().Count; i++)
        {
            weaponItemList.Add(new WeaponItemModel());
        }
    }

    public BigInteger GetWeaponDamage(WeaponTable table, int level)
    {
        return BigInteger.Parse(table.BaseAtk) + (level * BigInteger.Parse(table.Increase));
    }

    public void CheckWeaponState()
    {
        for (int i = 0; i < weaponItemList.Count; i++)
        {
            if (i < weaponItemList.Count - 1)
            {
                var curWeapon = weaponItemList[i];
                var nextWeapon = weaponItemList[i + 1];

                curWeapon.isEquiped = false;
                
                if (nextWeapon.level.Value < 1)
                {
                    curWeapon.isEquiped = true;
                    curWeapon.isUnLock = true;
                    nextWeapon.isUnLock = curWeapon.level.Value == 5;

                    return;
                }
            }
            else
            {
                var curWeapon = weaponItemList[i];
                curWeapon.isEquiped = curWeapon.level.Value > 0;
                curWeapon.isUnLock = true;
            }
        }
    }
}
