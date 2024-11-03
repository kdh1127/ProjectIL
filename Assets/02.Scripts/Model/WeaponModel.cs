using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Numerics;

public class WeaponItemModel
{
    public ReactiveProperty<int> level = new(0);
    public EnumList.EWeaponItemUpgradeStatus upgradeState;
    public bool isCurWeapon = false;

    public void Upgrade(WeaponTable table)
    {
        if (CurrencyManager.Instance.AddCurrency(EnumList.ECurrencyType.GOLD,-BigInteger.Parse(table.Cost)))
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
        WeaponTableList.Init(TRScriptableManager.Instance.GetGoogleSheet("WeaponTable"));

        for (int i = 0; i < WeaponTableList.Get().Count; i++)
        {
            weaponItemList.Add(new WeaponItemModel());
        }
    }

    public BigInteger GetWeaponDamage(WeaponTable table, int level)
    {
        BigInteger damage = BigInteger.Parse(table.BaseAtk) + (level * BigInteger.Parse(table.Increase));
        return damage;
    }

    public WeaponItemModel GetCurrentWeapon()
    {
        WeaponItemModel curWeapon = weaponItemList[0];

        weaponItemList.ForEach(weaponItem =>
        {
            bool isUpgrade = weaponItem.level.Value > 0;
            if (isUpgrade) curWeapon = weaponItem;
        });

        curWeapon.isCurWeapon = true;
        return curWeapon;
    }

    public void UpdateWeaponItemStatus()
    {
        if(weaponItemList[1].level.Value < 1)
        {
            weaponItemList[0].isCurWeapon = true;
            return;
        }

        for (int i = 0; i < weaponItemList.Count; i++)
        {
            if (weaponItemList[i].level.Value == 5)
            {
                weaponItemList[i].upgradeState = EnumList.EWeaponItemUpgradeStatus.MaxUpgrade;
            }
            else if (i > 1 && weaponItemList[i - 1].level.Value == 5)
            {
                weaponItemList[i].upgradeState = EnumList.EWeaponItemUpgradeStatus.Upgradeable;
            }
            else
            {
                weaponItemList[i].upgradeState = EnumList.EWeaponItemUpgradeStatus.NotUpgradeable;
            }
        }
    }
}
