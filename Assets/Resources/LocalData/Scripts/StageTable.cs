using UnityEngine;
using ThreeRabbitPackage;
using System.Collections.Generic;

public class StageTable
{
	[SerializeField] public int StageNo;
	[SerializeField] public string MonsterNormalName;
	[SerializeField] public string MonsterBossName;
	[SerializeField] public int HpIncreasePer;
	[SerializeField] public string Gold;
	[SerializeField] public string Dia;
	[SerializeField] public string Key;
public StageTable(int StageNo, string MonsterNormalName, string MonsterBossName, int HpIncreasePer, string Gold, string Dia, string Key)
	{
		this.StageNo = StageNo;
		this.MonsterNormalName = MonsterNormalName;
		this.MonsterBossName = MonsterBossName;
		this.HpIncreasePer = HpIncreasePer;
		this.Gold = Gold;
		this.Dia = Dia;
		this.Key = Key;
	}
}

public static class StageTableList
{
	public static List<StageTable> _StageTableList = new();
	public static void Init(TRGoogleSheet _trGoogleSheet)
	{
		_StageTableList.Clear();
		_trGoogleSheet.dataList.ForEach(data =>
		{
			StageTable _StageTable = new
			(
				StageNo: int.Parse(_trGoogleSheet.dataDictionary[data.key]["StageNo"]),
				MonsterNormalName: _trGoogleSheet.dataDictionary[data.key]["MonsterNormalName"],
				MonsterBossName: _trGoogleSheet.dataDictionary[data.key]["MonsterBossName"],
				HpIncreasePer: int.Parse(_trGoogleSheet.dataDictionary[data.key]["HpIncreasePer"]),
				Gold: _trGoogleSheet.dataDictionary[data.key]["Gold"],
				Dia: _trGoogleSheet.dataDictionary[data.key]["Dia"],
				Key: _trGoogleSheet.dataDictionary[data.key]["Key"]
			);
			_StageTableList.Add(_StageTable);
		});
	}

	public static List<StageTable> Get()
	{
		return _StageTableList;
	}
}
