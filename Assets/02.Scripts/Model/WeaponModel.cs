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
        if (CurrencyManager<Gold>.GetCurrency(ECurrencyType.GOLD).Subtract(table.Cost.ToBigInt()))
        {
            level.Value++;
            UpdateSubject.OnNext(table.WeaponNo);
            if (userWeaponUpgradeData.Keys.Contains(table.WeaponNo)) 
            {
                userWeaponUpgradeData[table.WeaponNo] = level.Value;
            }
        }
    }


    public void SetWeaponDamage(WeaponTable table)
    {
        var calcLevel = level.Value == 0 ? 0 : level.Value - 1;
        UserDataManager.Instance.characterData.WeaponDamage = BigInteger.Parse(table.BaseAtk) + (calcLevel * BigInteger.Parse(table.Increase));
    }
}

public class WeaponModel
{
    public List<WeaponItemModel> weaponItemList = new();

    public void Init(List<WeaponTable> weaponTableList)
    {
        for (int i = 0; i < weaponTableList.Count; i++)
        {
            weaponItemList.Add(new WeaponItemModel());
        }
    }

    public void Save()
    {
        DataUtility.Save("WeaponModel", this);
    }

    public void Load()
    {
        var data = DataUtility.Load<WeaponModel>("WeaponModel");

        data.weaponItemList.ForEach(weaponItem =>
        {
            weaponItemList.Add(weaponItem);
        });
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
