using UnityEngine;
using Zenject;
using ThreeRabbitPackage;

public class GameInitializer : MonoBehaviour
{
	[Inject] private readonly QuestPresenter questPresenter;
	[Inject] private readonly QuestModel questModel;

	[Inject] private readonly WeaponPresenter weaponPresenter;
	[Inject] private readonly WeaponModel weaponModel;

	[Inject] private readonly MissionPresenter missionPresenter;
	[Inject] private readonly MissionModel missionModel;

	[Inject] private readonly TreasurePresenter treasurePresenter;
	[Inject] private readonly TreasureModel treasureModel;

	[Inject] private readonly MainScenePresenter mainScenePresenter;

    private void Awake()
	{
		InitTable();
		InitModel();
	}

	private void Start()
	{
		SubscirbePresnters();
	}

	private void InitModel()
    {
		questModel.Init(QuestTableList.Get());
		weaponModel.Init(WeaponTableList.Get());
		missionModel.Init(MissionTableList.Get());
		treasureModel.Init(TreasureTableList.Get());
    }

	private void InitTable()
	{
		QuestTableList.Init(BindTable("QuestTable"));
		WeaponTableList.Init(BindTable("WeaponTable"));
		MissionTableList.Init(BindTable("MissionTable"));
		TreasureTableList.Init(BindTable("TreasureTable"));
	}

	private void SubscirbePresnters()
    {
		questPresenter.Subscribe();
		weaponPresenter.Subscribe();
		missionPresenter.Subscribe();
		treasurePresenter.Subscribe();
		mainScenePresenter.Subscribe();
	}

	private TRGoogleSheet BindTable(string sheetName)
	{
		return TRScriptableManager.Instance.GetGoogleSheet(sheetName);
	}
}
