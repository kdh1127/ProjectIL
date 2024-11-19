using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using ThreeRabbitPackage.DesignPattern;
using I2.Loc;
using AlphabetNumber;
using System.Numerics;
using System.Linq;

public class MainScenePresenter : TRSingleton<MainScenePresenter>
{
	public TopPanelView topPanelView;

	public MainButtonView mainButtonView;
	private MainButtonModel mainButtonModel = new();

	public BottomPanelView bottomPanelView;


	public MissionPanelView missionPanelView;
	private MissionModel missionModel = new();

	public TreasurePanelView treasurePanelView;
	private TreasureModel treasureModel = new();

	public CurrencyView currencyView;

	private void OnApplicationQuit()
	{
		//DataUtility.Save("WeaponModel", weaponModel);
	}
	private new void Awake()
	{
		base.Awake();

		Init();
	}

	private void Start()
	{
		Subscribe();
	}

	private void Init()
	{

		if (UserDataManager.Instance.IsInit())
		{
			//weaponModel.Load();
		}
		else
		{
			//weaponModel.Init(WeaponTableList.Get());
		}
		missionModel.Init();
		treasureModel.Init();
	}

	private void Subscribe()
	{
		TopPanelSubscribe();
		MainButtonSubscribe();
		CurrencySubscribe();
		MissionPanelSubscribe();
		TreasurePanelSubscribe();
	}

	private void TopPanelSubscribe()
	{
		StageManager.Instance.CurStage.Subscribe(curStage =>
		{
			topPanelView.stage_txt.text = StageManager.Instance.GetLocalizationStage(curStage);
		}).AddTo(this);
	}

	public void MainButtonSubscribe()
	{
		mainButtonModel.RegisterToggleList(mainButtonView.toggleGroup, mainButtonView.toggleList);
		mainButtonModel.toggleSubject.Subscribe(tgl =>
		{
			mainButtonView.DeactivateAllPanel(bottomPanelView.panelList);
			mainButtonView.ActivatePanel(tgl.type);
		}).AddTo(this);
	}

	public void CurrencySubscribe()
	{
		CurrencyManager<Gold>.GetCurrency(ECurrencyType.GOLD).Subscribe(gold =>
		{
			currencyView.gold_txt.text = gold.ToAlphabetNumber();
		}).AddTo(currencyView.gameObject);

		CurrencyManager<Dia>.GetCurrency(ECurrencyType.DIA).Subscribe(dia =>
		{
			currencyView.dia_txt.text = dia.ToAlphabetNumber();
		}).AddTo(currencyView.gameObject);

		CurrencyManager<Key>.GetCurrency(ECurrencyType.KEY).Subscribe(key =>
		{
			currencyView.key_txt.text = key.ToAlphabetNumber();
		}).AddTo(currencyView.gameObject);
	}



	public void MissionPanelSubscribe()
    {
		var curMissionTable = missionModel.GetCurMissionTable();
		var curMissionItemView = missionPanelView.currentMissionItemView;

		curMissionItemView.Init(curMissionTable, missionModel.IsClear(curMissionTable));

		curMissionItemView.completeButtonView.button.OnClickAsObservable().Subscribe(_ =>
		{
			missionModel.ClearMission(curMissionTable);
		}).AddTo(curMissionItemView.gameObject);

		missionModel.missionClearSubject.Subscribe(_ =>
		{
			var clearMissionNo = UserDataManager.Instance.missiondata.ClearMissionNo;

			curMissionTable = missionModel.GetCurMissionTable();
			curMissionItemView.UpdateView(curMissionTable, missionModel.IsClear(curMissionTable));
			missionPanelView.RefreshDisableMissionList(clearMissionNo);
		}).AddTo(missionPanelView.gameObject);

		missionPanelView.CreateDisableMissionList(UserDataManager.Instance.missiondata.ClearMissionNo);

		Observable.EveryUpdate().Subscribe(_ =>
		{
			missionPanelView.currentMissionItemView.UpdateView(curMissionTable, missionModel.IsClear(curMissionTable));
		}).AddTo(missionPanelView.gameObject);
    }

    public void TreasurePanelSubscribe()
    {
		var costImageResources = TRScriptableManager.Instance.GetSprite("CostImageResources").spriteDictionary;

		TreasureTableList.Get().ForEach(item =>
		{
			var treasureItemView = Instantiate(treasurePanelView.treasureItem, treasurePanelView.content_tr).GetComponent<TreasureItemView>();
			var treasureItemModel = treasureModel.treasureItemList[item.TreasureNo];

			treasureItemView.Init(item.TreasureName, item.IncreaseType, BigInteger.Parse(item.Increase), treasureItemModel.level.Value);
			treasureItemView.upgradeButtonView.Init(BigInteger.Parse(item.Increase), BigInteger.Parse(item.TreasureCost), costImageResources["Key"]);

		});
    }

}

