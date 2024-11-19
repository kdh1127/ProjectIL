using System;
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
    }

	private void InitTable()
	{
		QuestTableList.Init(BindTable("QuestTable"));
		WeaponTableList.Init(BindTable("WeaponTable"));
		MissionTableList.Init(BindTable("MissionTable"));
	}

	private void SubscirbePresnters()
    {
		questPresenter.Subscribe();
		weaponPresenter.WeaponPanelSubscribe();
		missionPresenter.MissionPanelSubscribe();
	}

	private TRGoogleSheet BindTable(string sheetName)
	{
		return TRScriptableManager.Instance.GetGoogleSheet(sheetName);
	}
}
