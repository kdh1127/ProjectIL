using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using ThreeRabbitPackage.DesignPattern;
using I2.Loc;
using AlphabetNumber;
using System.Numerics;

public class MainScenePresenter : TRSingleton<MainScenePresenter>
{
	public TopPanelView topPanelView;

	public MainButtonView mainButtonView;
	private MainButtonModel mainButtonModel = new();

	public BottomPanelView bottomPanelView;
	public QuestPanelView questPanelView;
	private QuestModel questModel = new();

	public WeaponPanelView weaponPanelView;
	private WeaponModel weaponModel = new();

	public MissionPanelView missionPanelView;
	private MissionModel missionModel = new();

	public TreasurePanelView treasurePanelView;
	private TreasureModel treasureModel = new();

	public CurrencyView currencyView;

	private new void Awake()
	{
		base.Awake();

		Init();
		Subscribe();
	}

	private void Init()
	{
		var questTableGoogleSheet = TRScriptableManager.Instance.GetGoogleSheet("QuestTable");
		QuestTableList.Init(questTableGoogleSheet);

		questModel.Init(QuestTableList.Get());
		weaponModel.Init();
		missionModel.Init();
		treasureModel.Init();
	}

	private void Subscribe()
	{
		TopPanelSubscribe();
		MainButtonSubscribe();
		CurrencySubscribe();
		QuestPanelSubscribe();
		WeaponPanelSubscribe();
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
		UserDataManager.Instance.currencyData.GetCurrency(EnumList.ECurrencyType.GOLD).Subscribe(gold =>
		{
			currencyView.gold_txt.text = gold.ToAlphabetNumber();
		}).AddTo(this);

		UserDataManager.Instance.currencyData.GetCurrency(EnumList.ECurrencyType.DIA).Subscribe(dia =>
		{
			currencyView.dia_txt.text = dia.ToAlphabetNumber();
		}).AddTo(this);

		UserDataManager.Instance.currencyData.GetCurrency(EnumList.ECurrencyType.KEY).Subscribe(key =>
		{
			currencyView.key_txt.text = key.ToAlphabetNumber();
		}).AddTo(this);
	}

	#region Quest
	public void QuestItemSubscribe(QuestTable table, QuestItemView view)
    {
        var model = questModel.questItemList[table.QuestNo];

		model.elpasedTime.Subscribe(time =>
		{
			view.ProgressUpdate(time, table.Time);
		}).AddTo(view.gameObject);

		model.level.Subscribe(level =>
		{
			view.UpdateLevel(level.ToString(), model.GetReward(table).ToAlphabetNumber());
		}).AddTo(view.gameObject);
    }

	public void QuestPanelSubscribe()
    {
		var questImageResources = TRScriptableManager.Instance.GetSprite("QuestImageResources").spriteDictionary;
		var costImageResources = TRScriptableManager.Instance.GetSprite("CostImageResources").spriteDictionary;

		QuestTableList.Get().ForEach(table =>
		{
			// Instantiate
			var view = Instantiate(questPanelView.questItem, questPanelView.content_tr).GetComponent<QuestItemView>();
			var model = questModel.questItemList[table.QuestNo];

			// Initialize 
			view.Init(
				sprite: questImageResources[table.Image],
				title: table.Name,
				endTime: table.Time,
				level: model.level.Value,
				reward: model.GetReward(table));

			view.upgradeButtonView.Init(
				increase: table.Increase.ToBigInteger(),
				cost: table.Cost.ToBigInteger(),
				costImage: costImageResources["Gold"]);

			// Subscribe QuestItemModel
			QuestItemSubscribe(table, view);

			// Subscribe Upgrade_btn
			view.upgradeButtonView.button.OnClickAsObservable().Subscribe(_ =>
			{
				questModel.questItemList[table.QuestNo].Upgrade(table);
			}).AddTo(view.upgradeButtonView.button.gameObject);

			// Subscribe currentGold
			UserDataManager.Instance.currencyData.GetCurrency(EnumList.ECurrencyType.GOLD).Subscribe(gold =>
			{
				view.upgradeButtonView.SetInteractable(gold >= table.Cost.ToBigInteger());
			}).AddTo(view.upgradeButtonView.button.gameObject);

			// Update Progress bar in Quest
			Observable.Interval(System.TimeSpan.FromSeconds(1))
			.Where(_ => model.IsOn)
			.Subscribe(_ =>
			{
				model.Progress(table);
			}).AddTo(view.gameObject);
		});
	}
	#endregion

	public void WeaponItemSubscribe(WeaponTable table, WeaponItemView weaponItemView)
	{
		var weaponItemModel = weaponModel.weaponItemList[table.WeaponNo];

		weaponItemModel.level.Subscribe(level =>
		{
			weaponModel.CheckWeaponState();
		}).AddTo(weaponItemView.gameObject);

		weaponItemModel.UpdateSubject.Subscribe(weaponNo =>
		{
			for (int i = 0; i < weaponPanelView.weaponItemViewList.Count; i++)
			{
				var isEnughGold = CurrencyManager.Instance.IsEnughCurrency(EnumList.ECurrencyType.GOLD, -BigInteger.Parse(WeaponTableList.Get()[i].Cost));
				var weaponItemModel = weaponModel.weaponItemList[i];
				var table = WeaponTableList.Get()[i];
				weaponPanelView.weaponItemViewList[i].UpdateLevel(
					table: table,
					curLevel: weaponItemModel.level.Value,
					isMaxLevel: weaponItemModel.IsMaxLevel,
					isEquiped: weaponItemModel.isEquiped,
					isUnLock: weaponItemModel.isUnLock,
					isEnughGold: isEnughGold
					);

				if (weaponItemModel.isEquiped)
                {
					weaponItemModel.SetWeaponDamage(table);

				}
			}
		}).AddTo(weaponPanelView.gameObject);
	}

	public void WeaponPanelSubscribe()
	{
		var costImageResources = TRScriptableManager.Instance.GetSprite("CostImageResources").spriteDictionary;

		weaponModel.CheckWeaponState();

		WeaponTableList.Get().ForEach(item =>
		{
			var weaponItemView = Instantiate(weaponPanelView.WeaponItem, weaponPanelView.content_tr).GetComponent<WeaponItemView>();
			var weaponItemModel = weaponModel.weaponItemList[item.WeaponNo];
			weaponPanelView.weaponItemViewList.Add(weaponItemView);

			if (item.WeaponNo == 0)
				if (weaponItemModel.level.Value == 0)
				{
					weaponItemModel.level.Value++;

					weaponItemModel.SetWeaponDamage(item);
				}

			weaponItemView.Init(
				table: item,
				curLevel: weaponItemModel.level.Value,
				isMaxLevel: weaponItemModel.IsMaxLevel,
				isEquiped: weaponItemModel.isEquiped,
				isUnLock: weaponItemModel.isUnLock,
				isEnughGold: CurrencyManager.Instance.IsEnughCurrency(EnumList.ECurrencyType.GOLD, -BigInteger.Parse(item.Cost))
				);

			weaponItemView.upgradeButtonView.Init(
				increase: BigInteger.Parse(item.Increase),
				cost: BigInteger.Parse(item.Cost),
				costImage: costImageResources["Gold"]
				);


			weaponItemView.upgradeButtonView.button.OnClickAsObservable().Subscribe(_ =>
            {
				weaponItemModel.Upgrade(item);
			}).AddTo(weaponItemView.gameObject);

			UserDataManager.Instance.currencyData.GetCurrency(EnumList.ECurrencyType.GOLD)
			.Subscribe(gold =>
			{
				var isEnughGold = CurrencyManager.Instance.IsPositiveAmount(gold, -BigInteger.Parse(item.Cost));
				weaponItemView.upgradeButtonView.SetInteractable(isEnughGold);
			}).AddTo(UserDataManager.Instance.gameObject);

			WeaponItemSubscribe(item, weaponItemView);
		});
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

