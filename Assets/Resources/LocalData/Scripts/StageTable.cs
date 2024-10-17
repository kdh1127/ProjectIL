using UnityEngine;
using ThreeRabbitPackage;
using System.Collections.Generic;

public class StageTable
{
	[SerializeField] public int StageNo;
	[SerializeField] public string MonsterNormalName;
	[SerializeField] public string MonsterBossName;
	[SerializeField] public int Hp;
	[SerializeField] public int Gold;
	[SerializeField] public int Crystal;
	[SerializeField] public int Key;
public StageTable(int StageNo, string MonsterNormalName, string MonsterBossName, int Hp, int Gold, int Crystal, int Key)
	{
		this.StageNo = StageNo;
		this.MonsterNormalName = MonsterNormalName;
		this.MonsterBossName = MonsterBossName;
		this.Hp = Hp;
		this.Gold = Gold;
		this.Crystal = Crystal;
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
				Hp: int.Parse(_trGoogleSheet.dataDictionary[data.key]["Hp"]),
				Gold: int.Parse(_trGoogleSheet.dataDictionary[data.key]["Gold"]),
				Crystal: int.Parse(_trGoogleSheet.dataDictionary[data.key]["Crystal"]),
				Key: int.Parse(_trGoogleSheet.dataDictionary[data.key]["Key"])
			);
			_StageTableList.Add(_StageTable);
		});
	}

	public static List<StageTable> Get()
	{
		return _StageTableList;
	}
}
