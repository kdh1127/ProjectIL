using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;

public class SkinItemModel
{
	public ReactiveProperty<int> m_level = new(0);
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
		var characterData = UserDataManager.Instance.characterData;
		var increaseType = table.IncreaseType.ToEnum<ESkinIncreaseType>();
		int totalIncrease = characterData.SkinStatDictionary[increaseType].ToInt();
		return m_level.Value <= 0 ? table.BaseValue : totalIncrease;
	}
}
public class SkinModel
{
	public readonly List<SkinItemModel> skinItemList = new();
	private readonly CurrencyModel.Dia dia;


	[Inject]
	public SkinModel(List<SkinItemModel> skinItemList, CurrencyModel.Dia dia)
	{
		this.skinItemList = skinItemList;
		this.dia = dia;
	}

	public void Init(List<SkinTable> tableList)
	{
		for (int i = 0; i < tableList.Count; i++)
		{
			skinItemList.Add(new SkinItemModel(tableList[i], dia));
		}
	}
}
