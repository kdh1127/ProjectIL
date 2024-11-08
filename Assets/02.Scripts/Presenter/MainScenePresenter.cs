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

	public CurrencyView currencyView;

	private new void Awake()
	{
		base.Awake();

		questModel.Init();
		weaponModel.Init();
		missionModel.Init();

		TopPanelSubscribe();
		MainButtonSubscribe();
		CurrencySubscribe();
		QuestPanelSubscribe();
		WeaponPanelSubscribe();
		MissionPanelSubscribe();
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
		UserDataManager.Instance.currencyData.Currency[EnumList.ECurrencyType.GOLD].Subscribe(gold =>
		{
			currencyView.gold_txt.text = gold.ToAlphabetNumber();
		}).AddTo(this);

		UserDataManager.Instance.currencyData.Currency[EnumList.ECurrencyType.DIA].Subscribe(dia =>
		{
			currencyView.dia_txt.text = dia.ToAlphabetNumber();
		}).AddTo(this);

		UserDataManager.Instance.currencyData.Currency[EnumList.ECurrencyType.KEY].Subscribe(key =>
		{
			currencyView.key_txt.text = key.ToAlphabetNumber();
		}).AddTo(this);
	}

	#region Quest
	public void QuestItemSubscribe(QuestTable table, QuestItemView questItemView)
    {
        var questItemModel = questModel.questItemList[table.QuestNo];

		questItemModel.elpasedTime.Subscribe(time =>
		{
			questItemView.ProgressUpdate(time, (int)table.Time);
		}).AddTo(questItemView.gameObject);

		questItemModel.level.Subscribe(level =>
		{
			questItemView.LevelUpdate(level.ToString(), questItemModel.GetReward(table).ToAlphabetNumber());
		}).AddTo(questItemView.gameObject);
    }

	public void QuestPanelSubscribe()
    {
		var questImageResources = TRScriptableManager.Instance.GetSprite("QuestImageResources").spriteDictionary;
		var costImageResources = TRScriptableManager.Instance.GetSprite("CostImageResources").spriteDictionary;

		QuestTableList.Get().ForEach(item =>
		{
			// Instantiate
			var questItemView = Instantiate(questPanelView.questItem, questPanelView.content_tr).GetComponent<QuestItemView>();
			var questItemModel = questModel.questItemList[item.QuestNo];

			// Initialize 
			questItemView.Init(
				sprite: questImageResources[item.Image],
				title: item.Name,
				endTime: item.Time,
				level: questItemModel.level.Value,
				reward: questItemModel.GetReward(item));

			questItemView.upgradeButtonView.Init(
				increase: new ANumber(item.Increase).ToAlphaString(),
				cost: new ANumber(item.Cost).ToAlphaString(),
				costImage: costImageResources["Gold"]);

			// Subscribe QuestItemModel
			QuestItemSubscribe(item, questItemView);

			// Subscribe Upgrade_btn
			questItemView.upgradeButtonView.button.OnClickAsObservable().Subscribe(_ =>
			{
				questModel.questItemList[item.QuestNo].Upgrade(item);
			}).AddTo(questItemView.gameObject);

			// Subscribe currentGold
			UserDataManager.Instance.currencyData.Currency[EnumList.ECurrencyType.GOLD].Subscribe(gold =>
			{
				questItemView.upgradeButtonView.SetInteractable(gold >= item.Cost);
			}).AddTo(questItemView.upgradeButtonView.button);

			// Update Progress bar in Quest
			Observable.Interval(System.TimeSpan.FromSeconds(1))
			.Where(_ => questItemModel.IsOn)
			.Subscribe(_ =>
			{
				questItemModel.Progress(item);
			}).AddTo(questItemView.gameObject);
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
					UserDataManager.Instance.characterData.WeaponDamage = BigInteger.Parse(table.BaseAtk) + (weaponItemModel.level.Value * BigInteger.Parse(table.Increase));
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

			weaponItemView.Init(
				table: item,
				curLevel: weaponItemModel.level.Value,
				isMaxLevel: weaponItemModel.IsMaxLevel,
				isEquiped: weaponItemModel.isEquiped,
				isUnLock: weaponItemModel.isUnLock,
				isEnughGold: CurrencyManager.Instance.IsEnughCurrency(EnumList.ECurrencyType.GOLD, -BigInteger.Parse(item.Cost))
				);

			weaponItemView.upgradeButtonView.Init(
				increase: item.Increase,
				cost: item.Cost,
				costImage: costImageResources["Gold"]
				);


			weaponItemView.upgradeButtonView.button.OnClickAsObservable().Subscribe(_ =>
            {
				weaponItemModel.Upgrade(item);
			}).AddTo(weaponItemView.gameObject);

			UserDataManager.Instance.currencyData.Currency[EnumList.ECurrencyType.GOLD]
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
		var clearMissionNo = UserDataManager.Instance.missiondata.ClearMissionNo;
		var missionTableList = MissionTableList.Get();
		var curMissionTable = missionModel.GetCurMissionTable();

		missionPanelView.currentMissionItemView.Init(
			table: curMissionTable,
			interactable: missionModel.IsClear(curMissionTable));

        for (int i = clearMissionNo + 1; i < missionTableList.Count; i++)
        {
			var missionItemView = Instantiate(missionPanelView.missionItem , missionPanelView.content_tr).GetComponent<MissionItemView>();
			missionItemView.Init(missionTableList[i]);	 
		}

		Observable.EveryUpdate().Subscribe(_ =>
		{
			missionPanelView.currentMissionItemView.Init(
				table: curMissionTable,
				interactable: missionModel.IsClear(curMissionTable));
		}).AddTo(missionPanelView.gameObject);
    }
}
