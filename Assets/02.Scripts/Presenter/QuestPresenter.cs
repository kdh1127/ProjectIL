using System;
using System.Collections.Generic;
using System.Numerics;
using UniRx;
using Zenject;

public class QuestPresenter
{
	private readonly QuestModel model;
	private readonly QuestPanelView view;
	private readonly QuestItemViewFactory questItemViewFactory;
	private readonly QuestResources resources;
	private readonly CurrencyModel.Gold gold;
	private readonly List<QuestTable> questTable;

	[Inject]
	public QuestPresenter(
		QuestModel model,
		QuestPanelView view,
		QuestItemViewFactory questItemViewFactory,
		QuestResources resources,
		List<QuestTable> questTable,
		CurrencyModel.Gold gold)
	{
		this.model = model;
		this.view = view;
		this.questItemViewFactory = questItemViewFactory;
		this.resources = resources;
		this.questTable = questTable;
		this.gold = gold;

	}

	public void Subscribe()
	{
		questTable.ForEach(table =>
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
		// Instantiate
		var itemView = questItemViewFactory.Create();
		var itemModel = model.questItemList[table.QuestNo];

		// Cache
		var cachedIncrease = table.Increase.ToBigInt();
		var cachedCost = table.Cost.ToBigInt();

		// Initialize 
		itemView.Init(
			sprite: resources.quest[table.Image],
			title: table.Name,
			endTime: table.Time,
			level: itemModel.m_level.Value,
			reward: itemModel.GetReward());

		itemView.upgradeButtonView.Init(
			increase: cachedIncrease,
			cost: cachedCost,
			costImage: resources.cost["Gold"]);

		return itemView;
	}

	private void SubscribeToUpgradeButton(QuestItemModel itemModel, QuestItemView itemView, BigInteger upgradeCost)
	{
		var upgradeButtonView = itemView.upgradeButtonView;

		upgradeButtonView.button.OnClickAsObservable()
		.Where(_ => gold.CanSubtract(upgradeCost))
		.Subscribe(_ => itemModel.IncreaseLevel())
		.AddTo(upgradeButtonView.gameObject);

		gold.Subscribe(gold => upgradeButtonView.SetInteractable(gold >= upgradeCost))
			.AddTo(upgradeButtonView.gameObject);
	}

	private void SubscribeToQuestItemModel(QuestItemModel itemModel, QuestItemView itemView, QuestTable table)
	{
		itemModel.m_elpasedTime
			.Subscribe(time =>	itemView.ProgressUpdate(time, table.Time))
			.AddTo(itemView.gameObject);

		itemModel.m_level.Subscribe(level =>
		{
			var reward = itemModel.GetReward().ToAlphabetNumber();
			itemView.UpdateLevel(level.ToString(), reward);
		}).AddTo(itemView.gameObject);
	}

	private void SubscribeToProgressBar(QuestItemModel itemModel, QuestItemView itemView, int endTime)
	{
		Observable.Interval(TimeSpan.FromSeconds(1))
		.Where(_ => itemModel.IsOn)
		.Subscribe(_ => itemModel.Progress(endTime))
		.AddTo(itemView.gameObject);
	}
}