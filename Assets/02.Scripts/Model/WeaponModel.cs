using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Numerics;
using System.Linq;
using Newtonsoft.Json;

public class WeaponItemModel
{
    public ReactiveProperty<int> m_level = new(0);
    public ReactiveProperty<bool> isMaxLevel = new();
    public ReactiveProperty<bool> isEquiped = new();
    public ReactiveProperty<bool> isUnLock = new();
    [JsonIgnore] public WeaponItemModel prevItemModel { get; set; }
    [JsonIgnore] public WeaponItemModel nextItemModel { get; set; }

    private readonly WeaponTable table;
    private readonly CurrencyModel.Gold gold;

    private UserDataManager.MissionData MissionData => UserDataManager.Instance.missionData;
    private UserDataManager.CharacterData CharacterData => UserDataManager.Instance.characterData;

    public WeaponItemModel(WeaponTable table, CurrencyModel.Gold gold)
	{
        this.table = table;
        this.gold = gold;
    }

	public void Upgrade()
    {
        if (gold.Subtract(table.Cost.ToBigInt()))
        {
            m_level.Value++;
            UpdateState();
            SetWeaponDamage();

            if (MissionData.WeaponUpgradeData.Keys.Contains(table.WeaponNo))
            {
                MissionData.WeaponUpgradeData[table.WeaponNo] = m_level.Value;
            }
        }
    }

    public void UpdateState()
    {
        if(nextItemModel != null)
		{
            if (nextItemModel.m_level.Value < 1)
            {
                isUnLock.Value = m_level.Value > 0;
                isMaxLevel.Value = m_level.Value == 5;
                isEquiped.Value = m_level.Value > 0;
                nextItemModel.isUnLock.Value = isMaxLevel.Value;
            }
        }

        if(prevItemModel != null)
		{
            if (isEquiped.Value)
			{
                prevItemModel.isEquiped.Value = false;
                UserDataManager.Instance.skinData.originWeaponNo = table.WeaponNo;
			}
            isUnLock.Value = prevItemModel.isMaxLevel.Value;
        }

        UserDataManager.Instance.weaponData.weaponItemList[table.WeaponNo].level = m_level.Value;
        UserDataManager.Instance.weaponData.weaponItemList[table.WeaponNo].isEquip = isEquiped.Value;
        UserDataManager.Instance.weaponData.weaponItemList[table.WeaponNo].isMaxLevel = isMaxLevel.Value;
        UserDataManager.Instance.weaponData.weaponItemList[table.WeaponNo].isUnlock = isUnLock.Value;
    }

    public void SetWeaponDamage()
    {
        var calcLevel = m_level.Value == 0 ? 0 : m_level.Value - 1;
        CharacterData.WeaponDamage = BigInteger.Parse(table.BaseAtk) + (calcLevel * BigInteger.Parse(table.Increase));
    }

    public void Reset()
    {
        m_level.Value = 0;
        isMaxLevel.Value = false;
        isUnLock.Value = false;
        isEquiped.Value = false;

        UserDataManager.Instance.weaponData.weaponItemList[table.WeaponNo].level = m_level.Value;
        UserDataManager.Instance.weaponData.weaponItemList[table.WeaponNo].isEquip = isEquiped.Value;
        UserDataManager.Instance.weaponData.weaponItemList[table.WeaponNo].isMaxLevel = isMaxLevel.Value;
        UserDataManager.Instance.weaponData.weaponItemList[table.WeaponNo].isUnlock = isUnLock.Value;
    }
}

public class WeaponModel
{
    public List<WeaponTable> weaponTableList = new();
    public List<WeaponItemModel> weaponItemList = new();
    private readonly CurrencyModel.Gold gold;

    public WeaponModel(List<WeaponTable> weaponTableList, CurrencyModel.Gold gold)
    {
        this.weaponTableList = weaponTableList;
        this.gold = gold;
    }
    public void Init()
    {
        var data = UserDataManager.Instance.weaponData.weaponItemList;

        for (int i = 0; i < WeaponTableList.Get().Count; i++)
        {
            weaponItemList.Add(new WeaponItemModel(weaponTableList[i], gold));
            weaponItemList[i].m_level.Value = data[i].level;
            weaponItemList[i].isUnLock.Value = data[i].isUnlock;
            weaponItemList[i].isMaxLevel.Value = data[i].isMaxLevel;
            weaponItemList[i].isEquiped.Value = data[i].isEquip;

            if (i > 0)
            {
                weaponItemList[i - 1].nextItemModel = weaponItemList[i]; // 마지막 인덱스는 null
                weaponItemList[i].prevItemModel = weaponItemList[i - 1]; // 첫번째 인덱스는 null
            }
        }

        // 첫 번째 무기 초기화
        var firstWeaponLevel = weaponItemList[0].m_level;
        if (firstWeaponLevel.Value == 0) firstWeaponLevel.Value = 1;
   

    }

    public void Reset()
    {
        var data = UserDataManager.Instance.weaponData.weaponItemList;

        weaponItemList.ForEach(weaponItem =>
        {
            weaponItem.Reset();
        });

        weaponItemList[0].m_level.Value = 1;
        weaponItemList[0].isEquiped.Value = true;
        weaponItemList[0].UpdateState();

        for (int i = 0; i < WeaponTableList.Get().Count; i++)
        {
            data[i].level = weaponItemList[i].m_level.Value;
            data[i].isUnlock = weaponItemList[i].isUnLock.Value;
            data[i].isMaxLevel = weaponItemList[i].isMaxLevel.Value;
            data[i].isEquip = weaponItemList[i].isEquiped.Value;
        }
    }
}
