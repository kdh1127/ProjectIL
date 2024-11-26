using UnityEngine;
using ThreeRabbitPackage;
using System.Collections.Generic;

public class WeaponTable
{
	[SerializeField] public int WeaponNo;
	[SerializeField] public string Name;
	[SerializeField] public string BaseAtk;
	[SerializeField] public string Increase;
	[SerializeField] public string Cost;
	[SerializeField] public string Image;
public WeaponTable(int WeaponNo, string Name, string BaseAtk, string Increase, string Cost, string Image)
	{
		this.WeaponNo = WeaponNo;
		this.Name = Name;
		this.BaseAtk = BaseAtk;
		this.Increase = Increase;
		this.Cost = Cost;
		this.Image = Image;
	}
}

public static class WeaponTableList
{
	public static List<WeaponTable> _WeaponTableList = new();
	public static void Init(TRGoogleSheet _trGoogleSheet)
	{
		_WeaponTableList.Clear();
		_trGoogleSheet.dataList.ForEach(data =>
		{
			WeaponTable _WeaponTable = new
			(
				WeaponNo: int.Parse(_trGoogleSheet.dataDictionary[data.key]["WeaponNo"]),
				Name: _trGoogleSheet.dataDictionary[data.key]["Name"],
				BaseAtk: _trGoogleSheet.dataDictionary[data.key]["BaseAtk"],
				Increase: _trGoogleSheet.dataDictionary[data.key]["Increase"],
				Cost: _trGoogleSheet.dataDictionary[data.key]["Cost"],
				Image: _trGoogleSheet.dataDictionary[data.key]["Image"]
			);
			_WeaponTableList.Add(_WeaponTable);
		});
	}

	public static List<WeaponTable> Get()
	{
		return _WeaponTableList;
	}
}
