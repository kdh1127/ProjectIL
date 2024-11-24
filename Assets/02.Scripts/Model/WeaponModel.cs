using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Numerics;
using System.Linq;

public class WeaponItemModel
{
    public ReactiveProperty<int> m_level = new(0);
    public ReactiveProperty<bool> isMaxLevel = new();
    public ReactiveProperty<bool> isEquiped = new();
    public ReactiveProperty<bool> isUnLock = new();
    public WeaponItemModel prevItemModel;
    public WeaponItemModel nextItemModel;

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
			}
		}
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
    }
}

public class WeaponModel
{
    public List<WeaponItemModel> weaponItemList = new();

    private readonly List<WeaponTable> weaponTableList;
    private readonly CurrencyModel.Gold gold;

    public WeaponModel(List<WeaponTable> weaponTableList, CurrencyModel.Gold gold)
	{
        this.weaponTableList = weaponTableList;
        this.gold = gold;
    }
    public void Init()
    {
        // 다음 웨폰 아이템 모델과 이전 웨폰 아이템 모델의 값을 변경시키기 위하여 더블 링크드 리스트로 초기화
        for(int i = 0; i < weaponTableList.Count; i++)
		{
            var weaponItemModel = new WeaponItemModel(weaponTableList[i], gold);
            weaponItemList.Add(weaponItemModel);

            if (i > 0)
            {
                weaponItemList[i - 1].nextItemModel = weaponItemModel; // 마지막 인덱스는 null
                weaponItemList[i].prevItemModel = weaponItemList[i - 1]; // 첫번째 인덱스는 null
            }
        }

        // 첫 번째 무기 초기화
        var firstWeaponLevel = weaponItemList[0].m_level;
        if (firstWeaponLevel.Value == 0) firstWeaponLevel.Value = 1;
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

    public void Reset()
    {
        weaponItemList.ForEach(weaponItem =>
        {
            weaponItem.Reset();
        });

        weaponItemList[0].m_level.Value = 1;
        weaponItemList[0].isEquiped.Value = true;
        weaponItemList[0].UpdateState();
    }
}
