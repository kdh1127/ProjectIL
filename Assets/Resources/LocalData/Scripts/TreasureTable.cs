using UnityEngine;
using ThreeRabbitPackage;
using System.Collections.Generic;

public class TreasureTable
{
	[SerializeField] public int TreasureNo;
	[SerializeField] public string TreasureName;
	[SerializeField] public string TreasureCost;
	[SerializeField] public string Increase;
	[SerializeField] public string IncreaseType;
public TreasureTable(int TreasureNo, string TreasureName, string TreasureCost, string Increase, string IncreaseType)
	{
		this.TreasureNo = TreasureNo;
		this.TreasureName = TreasureName;
		this.TreasureCost = TreasureCost;
		this.Increase = Increase;
		this.IncreaseType = IncreaseType;
	}
}

public static class TreasureTableList
{
	public static List<TreasureTable> _TreasureTableList = new();
	public static void Init(TRGoogleSheet _trGoogleSheet)
	{
		_TreasureTableList.Clear();
		_trGoogleSheet.dataList.ForEach(data =>
		{
			TreasureTable _TreasureTable = new
			(
				TreasureNo: int.Parse(_trGoogleSheet.dataDictionary[data.key]["TreasureNo"]),
				TreasureName: _trGoogleSheet.dataDictionary[data.key]["TreasureName"],
				TreasureCost: _trGoogleSheet.dataDictionary[data.key]["TreasureCost"],
				Increase: _trGoogleSheet.dataDictionary[data.key]["Increase"],
				IncreaseType: _trGoogleSheet.dataDictionary[data.key]["IncreaseType"]
			);
			_TreasureTableList.Add(_TreasureTable);
		});
	}

	public static List<TreasureTable> Get()
	{
		return _TreasureTableList;
	}
}
