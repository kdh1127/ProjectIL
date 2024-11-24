using UnityEngine;
using DG.Tweening;
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
	
	[Inject] private readonly CurrencyPresenter currencyPresenter;
	[Inject] private readonly CurrencyModel currencyModel;

	[Inject] private readonly ReincarnationPresenter reincarnationPresenter;
	[Inject] private readonly ReincarnationModel reincarnationModel;


	[Inject] private readonly SkinModel skinModel;
	[Inject] private readonly SkinPresenter skinPresenter;

	private void Awake()
	{
		InitPlugins();
		InitTable();
		InitModel();
	}

	private void Start()
	{
		SubscirbePresnters();
	}

	private void InitModel()
    {
		questModel.Init();
		weaponModel.Init();
		missionModel.Init();
		treasureModel.Init();
		currencyModel.Init();
		reincarnationModel.Init();
		skinModel.Init(SkinTableList.Get());
	}

	private void InitTable()
	{
		QuestTableList.Init(BindTable("QuestTable"));
		WeaponTableList.Init(BindTable("WeaponTable"));
		MissionTableList.Init(BindTable("MissionTable"));
		TreasureTableList.Init(BindTable("TreasureTable"));
		SkinTableList.Init(BindTable("SkinTable"));
	}

	private void SubscirbePresnters()
    {
		questPresenter.Subscribe();
		weaponPresenter.Subscribe();
		missionPresenter.Subscribe();
		treasurePresenter.Subscribe();
		mainScenePresenter.Subscribe();
		currencyPresenter.Subscribe();
		reincarnationPresenter.Subscribe();
		skinPresenter.Subscribe();
	}

	public void InitPlugins()
    {
		DOTween.Init();
	}
	private TRGoogleSheet BindTable(string sheetName)
	{
		return TRScriptableManager.Instance.GetGoogleSheet(sheetName);
	}
}
