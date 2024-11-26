using UnityEngine;
using ThreeRabbitPackage;
using System.Collections.Generic;

public class MissionTable
{
	[SerializeField] public int MissionNo;
	[SerializeField] public string Name;
	[SerializeField] public string MissionType;
	[SerializeField] public int TargetNo;
	[SerializeField] public int CompleteCount;
	[SerializeField] public string RewardType;
	[SerializeField] public string Amount;
public MissionTable(int MissionNo, string Name, string MissionType, int TargetNo, int CompleteCount, string RewardType, string Amount)
	{
		this.MissionNo = MissionNo;
		this.Name = Name;
		this.MissionType = MissionType;
		this.TargetNo = TargetNo;
		this.CompleteCount = CompleteCount;
		this.RewardType = RewardType;
		this.Amount = Amount;
	}
}

public static class MissionTableList
{
	public static List<MissionTable> _MissionTableList = new();
	public static void Init(TRGoogleSheet _trGoogleSheet)
	{
		_MissionTableList.Clear();
		_trGoogleSheet.dataList.ForEach(data =>
		{
			MissionTable _MissionTable = new
			(
				MissionNo: int.Parse(_trGoogleSheet.dataDictionary[data.key]["MissionNo"]),
				Name: _trGoogleSheet.dataDictionary[data.key]["Name"],
				MissionType: _trGoogleSheet.dataDictionary[data.key]["MissionType"],
				TargetNo: int.Parse(_trGoogleSheet.dataDictionary[data.key]["TargetNo"]),
				CompleteCount: int.Parse(_trGoogleSheet.dataDictionary[data.key]["CompleteCount"]),
				RewardType: _trGoogleSheet.dataDictionary[data.key]["RewardType"],
				Amount: _trGoogleSheet.dataDictionary[data.key]["Amount"]
			);
			_MissionTableList.Add(_MissionTable);
		});
	}

	public static List<MissionTable> Get()
	{
		return _MissionTableList;
	}
}
