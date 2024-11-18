using System;
using System.Numerics;
using UniRx;
using Zenject;

public class QuestPresenter
{
	private readonly QuestModel model;
	private readonly QuestPanelView view;
	private readonly QuestItemViewFactory questItemViewFactory;

	[Inject]
	public QuestPresenter(QuestModel model, QuestPanelView view, QuestItemViewFactory questItemViewFactory)
	{
		this.model = model;
		this.view = view;
		this.questItemViewFactory = questItemViewFactory;
	}

	public void Subscribe()
	{
		model.Init(QuestTableList.Get());

		QuestTableList.Get().ForEach(table =>
		{
			var itemView = CreateQuestItemView(table);
			var itemModel = model.questItemList[table.QuestNo];

			SubscribeToUpgradeButton(itemModel, itemView, table.Cost.ToBigInt());
			SubscribeToQuestItemModel(itemModel, itemView, table);
			SubscribeToProgressBar(itemModel, itemView, table.Time);
		});
	}

	public QuestItemView CreateQuestItemView(QuestTable table)
	{
		var questImageResources = TRScriptableManager.Instance.GetSprite("QuestImageResources").spriteDictionary;
		var costImageResources = TRScriptableManager.Instance.GetSprite("CostImageResources").spriteDictionary;

		// Instantiate
		var itemView = questItemViewFactory.Create();
		var itemModel = model.questItemList[table.QuestNo];

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

		return itemView;
	}

	private void SubscribeToUpgradeButton(QuestItemModel itemModel, QuestItemView itemView, BigInteger upgradeCost)
	{
		var gold = CurrencyManager<Gold>.GetCurrency(ECurrencyType.GOLD);

		itemView.upgradeButtonView.button.OnClickAsObservable()
		.Where(_ => gold.IsPositive(upgradeCost))
		.Subscribe(_ =>
		{
			gold.Sub(upgradeCost);
			itemModel.IncreaseLevel();
		}).AddTo(itemView.upgradeButtonView.gameObject);

		gold.Subscribe(gold =>
		{
			itemView.upgradeButtonView.SetInteractable(gold >= upgradeCost);
		}).AddTo(itemView.upgradeButtonView.gameObject);
	}

	private void SubscribeToQuestItemModel(QuestItemModel itemModel, QuestItemView itemView, QuestTable table)
	{
		var missionData = UserDataManager.Instance.missiondata;
		var cachedIncrease = table.Increase.ToBigInt();

		itemModel.elpasedTime.Subscribe(time =>
		{
			itemView.ProgressUpdate(time, table.Time);
		}).AddTo(itemView.gameObject);

		itemModel.level.Subscribe(level =>
		{
			itemView.UpdateLevel(level.ToString(), itemModel.GetReward(cachedIncrease).ToAlphabetNumber());
			missionData.UpdateQuestUpgradeData(table.QuestNo, currentValue => currentValue = level);
		}).AddTo(itemView.gameObject);

		itemModel.questClearSubject.Subscribe(_ =>
		{
			CurrencyManager<Gold>.GetCurrency(ECurrencyType.GOLD).Add(itemModel.GetReward(cachedIncrease));
			missionData.UpdateQuestClearData(table.QuestNo, currentValue => currentValue + 1);
		});
	}

	private void SubscribeToProgressBar(QuestItemModel itemModel, QuestItemView itemView, int endTime)
	{
		Observable.Interval(TimeSpan.FromSeconds(1))
		.Where(_ => itemModel.IsOn)
		.Subscribe(_ =>
		{
			itemModel.Progress(endTime);
		}).AddTo(itemView.gameObject);
	}
}