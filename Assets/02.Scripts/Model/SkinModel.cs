using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;

public class SkinItemModel
{
	public ReactiveProperty<int> m_level = new(0);
	public ReactiveProperty<bool> isEquip = new();
	public Subject<Unit> updateSubject = new();
	public bool IsBought => m_level.Value > 0;

	private readonly SkinTable table;
	private readonly CurrencyModel.Dia dia;


	[Inject]
	public SkinItemModel (SkinTable table, CurrencyModel.Dia dia)
	{
		this.table = table;
		this.dia = dia;
	}

	public void IncreaseLevel()
	{
		var cost = IsBought ? table.UpgradeCost : table.buyCost;

		if (!dia.Subtract(cost)) return;

		// TODO: ReactiveProperty의 Value는 프로퍼티이므로 Set에서 OnNext를 호출하고 Subscribe로 바로 넘어간다.
		// 따라서 UpdateSkinStatus를 먼저 호출해주어야 원하는대로 동작하며 호출 순서에 주의해야한다.
		// Subject로 해결하려 하였으나 마찬가지로 안된다.
		// subscribe의 이벤트 흐름 제어 관련하여 찾아봐야한다.
		UpdateSkinStatus();
		m_level.Value++;

		UserDataManager.Instance.skinData.skinList[table.SkinNo].level = m_level.Value;
		UserDataManager.Instance.missionData.UpdateBuySkinData(0, currentValue => currentValue + 1);
	}

	public void UpdateEquipSkin(bool isEquip)
	{
		this.isEquip.Value = isEquip;
		UserDataManager.Instance.skinData.skinList[table.SkinNo].isEquip = this.isEquip.Value;
	}
	public void UpdateSkinStatus()
	{
		var characterData = UserDataManager.Instance.characterData;
		var increaseType = table.IncreaseType.ToEnum<ESkinIncreaseType>();
		var incraseValue = IsBought ? table.UpgradeValue : table.BaseValue;

		characterData.SkinStatDictionary[increaseType] += incraseValue;
	}

	public int GetTotalIncrease()
    {
		int totalIncrease = table.BaseValue + ((m_level.Value -1) * table.UpgradeValue);
		return m_level.Value > 1 ? totalIncrease : table.BaseValue;
	}
}
public class SkinModel
{
	public readonly List<SkinTable> tableList;
	private readonly CurrencyModel.Dia dia;
	public List<SkinItemModel> skinItemList = new();
	public int OriginWeaponNo => UserDataManager.Instance.skinData.originWeaponNo;
	public int EquipSkinNo => UserDataManager.Instance.skinData.equipSkinNo;

	[Inject]
	public SkinModel(List<SkinTable> tableList, CurrencyModel.Dia dia)
	{
		this.tableList = tableList;
		this.dia = dia;
	}

	public void Init()
	{
		var data = UserDataManager.Instance.skinData;

		for (int i = 0; i < tableList.Count; i++)
		{
			var skinData = new SkinItemModel(tableList[i], dia);
			skinData.m_level.Value = data.skinList[i].level;
			skinData.isEquip.Value = data.skinList[i].isEquip;
			skinItemList.Add(skinData);
		}
	}

	public bool IsUnEquipAllSkin()
	{
		var isEquipSkin = false;
		skinItemList.ForEach(skinItem =>
		{
			if (skinItem.isEquip.Value) isEquipSkin = true;
		});
		return isEquipSkin;
	}
}
