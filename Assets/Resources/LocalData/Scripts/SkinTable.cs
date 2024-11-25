using UnityEngine;
using ThreeRabbitPackage;
using System.Collections.Generic;

public class SkinTable
{
	[SerializeField] public int SkinNo;
	[SerializeField] public string SkinImageNo;
	[SerializeField] public string SkinName;
	[SerializeField] public string IncreaseType;
	[SerializeField] public int buyCost;
	[SerializeField] public int UpgradeCost;
	[SerializeField] public int BaseValue;
	[SerializeField] public int UpgradeValue;
	[SerializeField] public string IncreaseTypeString;
public SkinTable(int SkinNo, string SkinImageNo, string SkinName, string IncreaseType, int buyCost, int UpgradeCost, int BaseValue, int UpgradeValue, string IncreaseTypeString)
	{
		this.SkinNo = SkinNo;
		this.SkinImageNo = SkinImageNo;
		this.SkinName = SkinName;
		this.IncreaseType = IncreaseType;
		this.buyCost = buyCost;
		this.UpgradeCost = UpgradeCost;
		this.BaseValue = BaseValue;
		this.UpgradeValue = UpgradeValue;
		this.IncreaseTypeString = IncreaseTypeString;
	}
}

public static class SkinTableList
{
	public static List<SkinTable> _SkinTableList = new();
	public static void Init(TRGoogleSheet _trGoogleSheet)
	{
		_SkinTableList.Clear();
		_trGoogleSheet.dataList.ForEach(data =>
		{
			SkinTable _SkinTable = new
			(
				SkinNo: int.Parse(_trGoogleSheet.dataDictionary[data.key]["SkinNo"]),
				SkinImageNo: _trGoogleSheet.dataDictionary[data.key]["SkinImageNo"],
				SkinName: _trGoogleSheet.dataDictionary[data.key]["SkinName"],
				IncreaseType: _trGoogleSheet.dataDictionary[data.key]["IncreaseType"],
				buyCost: int.Parse(_trGoogleSheet.dataDictionary[data.key]["buyCost"]),
				UpgradeCost: int.Parse(_trGoogleSheet.dataDictionary[data.key]["UpgradeCost"]),
				BaseValue: int.Parse(_trGoogleSheet.dataDictionary[data.key]["BaseValue"]),
				UpgradeValue: int.Parse(_trGoogleSheet.dataDictionary[data.key]["UpgradeValue"]),
				IncreaseTypeString: _trGoogleSheet.dataDictionary[data.key]["IncreaseTypeString"]
			);
			_SkinTableList.Add(_SkinTable);
		});
	}

	public static List<SkinTable> Get()
	{
		return _SkinTableList;
	}
}
