using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class StageModel : MonoBehaviour
{
	public ReactiveProperty<int> CurStage = new();

	private void Awake()
	{
		if(StageManager.Instance != null)
			StageTableList.Init(StageManager.Instance.stageTable);
	}

	public StageTable GetCurStageData()
	{
		var stageTableList = StageTableList.Get();
		var stageTableCount = stageTableList.Count;

		for (int i = 0; i < stageTableCount; i++)
		{
			if(CurStage.Value < stageTableList[i].StageNo)
			{
				return stageTableList[i - 1];
			}
		}


		return stageTableList[stageTableCount - 1];
	}
}
