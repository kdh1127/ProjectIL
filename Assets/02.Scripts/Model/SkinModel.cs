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

		// TODO: ReactiveProperty�� Value�� ������Ƽ�̹Ƿ� Set���� OnNext�� ȣ���ϰ� Subscribe�� �ٷ� �Ѿ��.
		// ���� UpdateSkinStatus�� ���� ȣ�����־�� ���ϴ´�� �����ϸ� ȣ�� ������ �����ؾ��Ѵ�.
		// Subject�� �ذ��Ϸ� �Ͽ����� ���������� �ȵȴ�.
		// subscribe�� �̺�Ʈ �帧 ���� �����Ͽ� ã�ƺ����Ѵ�.
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
