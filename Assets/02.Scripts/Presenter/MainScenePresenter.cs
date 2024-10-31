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
	private StageModel stageModel = new();

	public MainButtonView mainButtonView;
	private MainButtonModel mainButtonModel = new();

	public BottomPanelView bottomPanelView;

	public QuestPanelView questPanelView;
	private QuestModel questModel = new();

	public WeaponPanelView weaponPanelView;
	private WeaponModel weaponModel = new();

	public CurrencyView currencyView;

	private void Awake()
	{
		base.Awake();

		questModel.Init();
		weaponModel.Init();

		TopPanelSubscribe();
		MainButtonSubscribe();
		CurrencySubscribe();
		QuestPanelSubscribe();
		WeaponPanelSubscribe();
	}

	private void TopPanelSubscribe()
	{
		stageModel.CurStage.Subscribe(curStage =>
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
		CurrencyManager.Instance.gold.Subscribe(gold =>
		{
			currencyView.gold_txt.text = gold.ToAlphaString();
		}).AddTo(this);

		CurrencyManager.Instance.dia.Subscribe(dia =>
		{
			currencyView.dia_txt.text = dia.ToAlphaString();
		}).AddTo(this);

		CurrencyManager.Instance.key.Subscribe(key =>
		{
			currencyView.key_txt.text = key.ToAlphaString();
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
			questItemView.LevelUpdate(level.ToString(), questItemModel.GetReward(table).ToString());
		}).AddTo(questItemView.gameObject);
	}

	public void QuestPanelSubscribe()
	{
		var questImageResources = TRScriptableManager.Instance.Sprite["QuestImage"].spriteDictionary;
		var costImageResources = TRScriptableManager.Instance.Sprite["CostImage"].spriteDictionary;

		QuestTableList.Get().ForEach(item =>
		{
			// Instantiate
			var qusetitemView = Instantiate(questPanelView.questItem, questPanelView.content_tr).GetComponent<QuestItemView>();
			var questItemModel = questModel.questItemList[item.QuestNo];

			// Initialize 
			qusetitemView.Init(
				sprite: questImageResources[item.Image],
				title: item.Name,
				endTime: item.Time,
				level: questItemModel.level.Value,
				reward: questItemModel.GetReward(item));

			qusetitemView.upgradeButtonView.Init(
				increase: new ANumber(item.Increase).ToAlphaString(),
				cost: new ANumber(item.Cost).ToAlphaString(),
				costImage: costImageResources["Gold"]);

			// Subscribe QuestItemModel
			QuestItemSubscribe(item, qusetitemView);

			// Subscribe Upgrade_btn
			qusetitemView.upgradeButtonView.button.OnClickAsObservable().Subscribe(_ =>
			{
				questModel.questItemList[item.QuestNo].Upgrade(item);
			}).AddTo(qusetitemView.gameObject);

			// Subscribe currentGold
			CurrencyManager.Instance.gold.Subscribe(gold =>
			{
				qusetitemView.upgradeButtonView.SetInteractable(gold.bigInteger >= item.Cost);
			}).AddTo(qusetitemView.upgradeButtonView.button);

			// Update Progress bar in Quest
			Observable.Interval(System.TimeSpan.FromSeconds(1))
			.Where(_ => questItemModel.IsOn)
			.Subscribe(_ =>
			{
				questItemModel.Progress(item);
			}).AddTo(qusetitemView.gameObject);

		});
	}
	#endregion

	public void WeaponPanelSubscribe()
	{
		var costImageResources = TRScriptableManager.Instance.Sprite["CostImage"].spriteDictionary;

		WeaponTableList.Get().ForEach(item =>
		{
			var weaponItemView = Instantiate(weaponPanelView.WeaponItem, weaponPanelView.content_tr).GetComponent<WeaponItemView>();
			var weaponItemModel = weaponModel.weaponItemList[item.WeaponNo];
			Debug.Log(item.Cost);
			weaponItemView.Init(
				title: item.Name,
				level: weaponItemModel.level.Value,
				baseAtk: BigInteger.Parse(item.BaseAtk)
				);

			weaponItemView.upgradeButtonView.Init(
				increase: item.Increase,
				cost: item.Cost,
				costImage: costImageResources["Gold"]
				);

			weaponItemView.upgradeButtonView.button.OnClickAsObservable().Subscribe(_ =>
			{

				weaponModel.weaponItemList[item.WeaponNo].Upgrade(item);
				weaponItemView.LevelUpdate(BigInteger.Parse(item.BaseAtk) + BigInteger.Parse(item.Increase), weaponItemModel.level.Value);
			}).AddTo(weaponItemView.gameObject);


			CurrencyManager.Instance.gold.Subscribe(gold =>
			{
				weaponItemView.upgradeButtonView.SetInteractable(gold.bigInteger >= BigInteger.Parse(item.Cost));
			}).AddTo(weaponItemView.upgradeButtonView.button);

			weaponModel.weaponItemList[item.WeaponNo].level.Subscribe(level =>
			{
				if (weaponModel.weaponItemList[item.WeaponNo].level.Value > 4)
				{
					weaponItemView.upgradeButtonView.button.interactable = false;
					return;
				}

			}).AddTo(weaponItemView.gameObject);
		});
	}
}
