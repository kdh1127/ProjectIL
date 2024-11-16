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
	public QuestPanelView questPanelView;
	private QuestModel questModel = new();

	public WeaponPanelView weaponPanelView;
	private WeaponModel weaponModel = new();

	public MissionPanelView missionPanelView;
	private MissionModel missionModel = new();

	public TreasurePanelView treasurePanelView;
	private TreasureModel treasureModel = new();

	public CurrencyView currencyView;

	private void OnApplicationQuit()
	{
		DataUtility.Save("QuestModel", questModel);
		DataUtility.Save("WeaponModel", weaponModel);
	}
	private new void Awake()
	{
		base.Awake();

		Init();
		Subscribe();
	}

	private void Init()
	{
		QuestTableList.Init(TRScriptableManager.Instance.GetGoogleSheet("QuestTable"));
		WeaponTableList.Init(TRScriptableManager.Instance.GetGoogleSheet("WeaponTable"));

		if (UserDataManager.Instance.IsInit())
		{
			questModel.Load();
			weaponModel.Load();
		}
		else
		{
			questModel.Init(QuestTableList.Get());
			weaponModel.Init(WeaponTableList.Get());
		}
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
		UserDataManager.Instance.currencyData.GetCurrency(ECurrencyType.GOLD).Subscribe(gold =>
		{
			currencyView.gold_txt.text = gold.ToAlphabetNumber();
		}).AddTo(this);

		UserDataManager.Instance.currencyData.GetCurrency(ECurrencyType.DIA).Subscribe(dia =>
		{
			currencyView.dia_txt.text = dia.ToAlphabetNumber();
		}).AddTo(this);

		UserDataManager.Instance.currencyData.GetCurrency(ECurrencyType.KEY).Subscribe(key =>
		{
			currencyView.key_txt.text = key.ToAlphabetNumber();
		}).AddTo(this);
	}

	#region Quest
	public void QuestPanelSubscribe()
    {
		var questImageResources = TRScriptableManager.Instance.GetSprite("QuestImageResources").spriteDictionary;
		var costImageResources = TRScriptableManager.Instance.GetSprite("CostImageResources").spriteDictionary;
		var missionData = UserDataManager.Instance.missiondata;

		QuestTableList.Get().ForEach(table =>
		{
			// Instantiate
			var itemView = Instantiate(questPanelView.questItem, questPanelView.content_tr).GetComponent<QuestItemView>();
			var itemModel = questModel.questItemList[table.QuestNo];

			// Cache
			var cachedIncrease = table.Increase.ToBigInt();
			var cachedCost = table.Cost.ToBigInt();
			var cachedTime = table.Time;

			// Initialize 
			itemView.Init(
				sprite: questImageResources[table.Image],
				title: table.Name,
				endTime: cachedTime,
				level: itemModel.level.Value,
				reward: itemModel.GetReward(cachedIncrease));

			itemView.upgradeButtonView.Init(
				increase: cachedIncrease,
				cost: cachedCost,
				costImage: costImageResources["Gold"]);

			// Subscribe Upgrade_btn
			itemView.upgradeButtonView.button.OnClickAsObservable()
			.Where(_ => CurrencyManager.Instance.IsEnughCurrency(ECurrencyType.GOLD, cachedCost))
			.Subscribe(_ =>
			{
				itemModel.IncreaseLevel();
			}).AddTo(itemView.upgradeButtonView.button.gameObject);

			// Subscribe QuestItemModel
			itemModel.elpasedTime.Subscribe(time =>
			{
				itemView.ProgressUpdate(time, cachedTime);
			}).AddTo(itemView.gameObject);

			itemModel.level.Subscribe(level =>
			{
				itemView.UpdateLevel(level.ToString(), itemModel.GetReward(cachedIncrease).ToAlphabetNumber());
				missionData.UpdateQuestUpgradeData(table.QuestNo, currentValue => currentValue = level);
			}).AddTo(itemView.gameObject);

			itemModel.questClearSubject.Subscribe(_ =>
			{
				CurrencyManager.Instance.AddCurrency(ECurrencyType.GOLD, itemModel.GetReward(cachedIncrease));
				missionData.UpdateQuestClearData(table.QuestNo, currentValue => currentValue + 1);
			});

			// Subscribe currentGold
			UserDataManager.Instance.currencyData.GetCurrency(ECurrencyType.GOLD).Subscribe(gold =>
			{
				itemView.upgradeButtonView.SetInteractable(gold >= cachedCost);
			}).AddTo(itemView.upgradeButtonView.button.gameObject);

			// Update Progress bar in Quest
			Observable.Interval(System.TimeSpan.FromSeconds(1))
			.Where(_ => itemModel.IsOn)
			.Subscribe(_ =>
			{
				itemModel.Progress(cachedTime);
			}).AddTo(itemView.gameObject);
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
				var isEnughGold = CurrencyManager.Instance.IsEnughCurrency(ECurrencyType.GOLD, -BigInteger.Parse(WeaponTableList.Get()[i].Cost));
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
				isEnughGold: CurrencyManager.Instance.IsEnughCurrency(ECurrencyType.GOLD, -BigInteger.Parse(item.Cost))
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

			UserDataManager.Instance.currencyData.GetCurrency(ECurrencyType.GOLD)
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

