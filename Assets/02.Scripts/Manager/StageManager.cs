using I2.Loc;
using System.Numerics;
using ThreeRabbitPackage.DesignPattern;
using UniRx;
using System.Linq;

public class StageManager : TRSingleton<StageManager>
{
	public ReactiveProperty<int> CurStage = new(0);
	public BigInteger stageBaseHp = 0;
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
		var stageFormat = LocalizationManager.GetTranslation("Stage_String_Format");
        var stageString = string.Format(stageFormat, curStage);
        return stageString;
    }

	public int GetCurStep()
	{
		 return MonsterManager.Instance.monsterIndex + 1;
	}

	public void IncreaseStage()
	{
		var userDungeonClearData = UserDataManager.Instance.missiondata.DungeonClearData;
		CurStage.Value++;

		if (userDungeonClearData.Keys.Contains(0))
        {
			userDungeonClearData[0] = CurStage.Value;
        }
	}

	public void SetStageBaseHp(BigInteger prevBaseHp)
	{
		if (CurStage.Value == 0)
		{
			stageBaseHp = 500;
			return;
		}

		var increaseHp = (prevBaseHp * CurStage.Value * GetCurStageTable().HpIncreasePer) / 100;
		stageBaseHp = prevBaseHp + increaseHp;
	}

}
