using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThreeRabbitPackage;
using ThreeRabbitPackage.DesignPattern;
using UniRx;

public class StageManager : TRSingleton<StageManager>
{
	public ReactiveProperty<int> CurStage = new(0);

	private new void Awake()
	{
		base.Awake();
		var table = TRScriptableManager.Instance.GetGoogleSheet("StageTable");
		if (TRScriptableManager.Instance != null)
			StageTableList.Init(table);
	}

	public StageTable GetCurStageTable()
	{
		var stageTableList = StageTableList.Get();
		if (CurStage.Value == 0) return stageTableList[0];

		for (int i = 0; i < stageTableList.Count; i++)
		{
			if (CurStage.Value < stageTableList[i].StageNo)
			{
				return stageTableList[i - 1];
			}
		}

		return stageTableList[stageTableList.Count - 1];
	}

	public string GetLocalizationStage(int curStage)
    {
        var stageFormat = ScriptLocalization.Localization.Stage;
        var stageString = string.Format(stageFormat, curStage);
        return stageString;
    }

	public int GetCurStep()
	{
		 return MonsterManager.Instance.monsterIndex + 1;
	}

	public void IncreaseStage()
	{
		CurStage.Value++;
	}
}
