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

	public CurrencyView currencyView;

	private new void Awake()
	{
		base.Awake();

		questModel.Init();
		weaponModel.Init();

		TopPanelSubscribe();
		MainButtonSubscribe();
		CurrencySubscribe();
		QuestPanelSubscribe();
		WeaponPanelSubscribe();

		weaponModel.weaponItemList[0].level.Value = 1;
		weaponModel.GetCurrentWeapon();
		weaponModel.UpdateWeaponItemStatus();
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
			weaponItemView.UpdateLevel(table, level);
			weaponModel.GetCurrentWeapon();
			weaponModel.UpdateWeaponItemStatus();
		}).AddTo(weaponItemView.gameObject);
	}
	public void WeaponPanelSubscribe()
	{
		var costImageResources = TRScriptableManager.Instance.GetSprite("CostImageResources").spriteDictionary;

		WeaponTableList.Get().ForEach(item =>
		{
			var weaponItemView = Instantiate(weaponPanelView.WeaponItem, weaponPanelView.content_tr).GetComponent<WeaponItemView>();
			var weaponItemModel = weaponModel.weaponItemList[item.WeaponNo];

			weaponItemView.Init(
				table: item,
				curLevel: weaponItemModel.level.Value
				);

			weaponItemView.upgradeButtonView.Init(
				increase: item.Increase,
				cost: item.Cost,
				costImage: costImageResources["Gold"]
				);

			WeaponItemSubscribe(item, weaponItemView);

			weaponItemView.upgradeButtonView.button.OnClickAsObservable().Subscribe(_ =>
            {
				var curGold = UserDataManager.Instance.currencyData.Currency[EnumList.ECurrencyType.GOLD].Value;
				var isEnughGold = CurrencyManager.Instance.IsPositiveAmount(curGold, BigInteger.Parse(item.Cost));
                weaponModel.weaponItemList[item.WeaponNo].Upgrade(item);

				weaponItemView.UpdateListView(weaponItemModel.isCurWeapon);
				weaponItemView.UpdateWeaponViewStatus(weaponItemModel.upgradeState, isEnughGold);
			}).AddTo(weaponItemView.gameObject);

			UserDataManager.Instance.currencyData.Currency[EnumList.ECurrencyType.GOLD]
			.Where(_ => weaponItemModel.isCurWeapon)
			.Subscribe(gold =>
			{
				var curGold = UserDataManager.Instance.currencyData.Currency[EnumList.ECurrencyType.GOLD].Value;
				var isEnughGold = CurrencyManager.Instance.IsPositiveAmount(curGold, BigInteger.Parse(item.Cost));
				weaponItemView.upgradeButtonView.SetInteractable(isEnughGold);
			}).AddTo(UserDataManager.Instance.gameObject);
        });
	}
}
