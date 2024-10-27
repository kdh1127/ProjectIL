using UnityEngine;
using ThreeRabbitPackage;
using System.Collections.Generic;

public class QuestTable
{
	[SerializeField] public int QuestNo;
	[SerializeField] public string Name;
	[SerializeField] public float Time;
	[SerializeField] public int Increase;
	[SerializeField] public int Cost;
	[SerializeField] public string Image;
public QuestTable(int QuestNo, string Name, float Time, int Increase, int Cost, string Image)
	{
		this.QuestNo = QuestNo;
		this.Name = Name;
		this.Time = Time;
		this.Increase = Increase;
		this.Cost = Cost;
		this.Image = Image;
	}
}

public static class QuestTableList
{
	public static List<QuestTable> _QuestTableList = new();
	public static void Init(TRGoogleSheet _trGoogleSheet)
	{
		_QuestTableList.Clear();
		_trGoogleSheet.dataList.ForEach(data =>
		{
			QuestTable _QuestTable = new
			(
				QuestNo: int.Parse(_trGoogleSheet.dataDictionary[data.key]["QuestNo"]),
				Name: _trGoogleSheet.dataDictionary[data.key]["Name"],
				Time: float.Parse(_trGoogleSheet.dataDictionary[data.key]["Time"]),
				Increase: int.Parse(_trGoogleSheet.dataDictionary[data.key]["Increase"]),
				Cost: int.Parse(_trGoogleSheet.dataDictionary[data.key]["Cost"]),
				Image: _trGoogleSheet.dataDictionary[data.key]["Image"]
			);
			_QuestTableList.Add(_QuestTable);
		});
	}

	public static List<QuestTable> Get()
	{
		return _QuestTableList;
	}
}
