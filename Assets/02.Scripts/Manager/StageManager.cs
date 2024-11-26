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
	private void Start()
	{
		var stage = UserDataManager.Instance.stageData.stage;
		CurStage.Value = stage.curStage;
		stageBaseHp = stage.stageBaseHp;
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
		var curStep = MonsterManager.Instance.monsterIndex + 1;

		return curStep;
	}

	public void IncreaseStage()
	{
		var userDungeonClearData = UserDataManager.Instance.missionData.DungeonClearData;
		CurStage.Value++;

		if (userDungeonClearData.Keys.Contains(0))
        {
			userDungeonClearData[0] = CurStage.Value;
        }
		var stage = UserDataManager.Instance.stageData.stage;
		stage.curStage = CurStage.Value;
	}

	public void SetStageBaseHp(BigInteger prevBaseHp)
	{
		var stage = UserDataManager.Instance.stageData.stage;
		stage.stageBaseHp = prevBaseHp;
		if (CurStage.Value == 0)
		{
			stageBaseHp = 500;
			return;
		}

		var increaseHp = (500 * CurStage.Value * GetCurStageTable().HpIncreasePer) / 100;
		stageBaseHp = prevBaseHp + increaseHp;
	}

    public void ResetStage()
    {
		CurStage.Value = 0;
		stageBaseHp = 0;

		var stage = UserDataManager.Instance.stageData.stage;

		stage.stageBaseHp = stageBaseHp;
		stage.curStage = CurStage.Value;
	}

}
