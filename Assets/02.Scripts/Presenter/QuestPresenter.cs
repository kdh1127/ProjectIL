using System;
using System.Numerics;
using UniRx;
using UnityEngine;
using Zenject;
using System.Collections.Generic;
public class QuestPresenter
{
	private readonly QuestModel model;
	private readonly QuestPanelView view;
	private readonly QuestItemViewFactory questItemViewFactory;
	private readonly QuestResources resource;

	[Inject]
	public QuestPresenter(QuestModel model, QuestPanelView view, QuestItemViewFactory questItemViewFactory, QuestResources resource)
	{
		this.model = model;
		this.view = view;
		this.questItemViewFactory = questItemViewFactory;
		this.resource = resource;

	}

	public void Subscribe()
	{
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
		// Instantiate
		var itemView = questItemViewFactory.Create();
		var itemModel = model.questItemList[table.QuestNo];

		// Cache
		var cachedIncrease = table.Increase.ToBigInt();
		var cachedCost = table.Cost.ToBigInt();

		// Initialize 
		itemView.Init(
			sprite: resource.quest[table.Image],
			title: table.Name,
			endTime: table.Time,
			level: itemModel.level.Value,
			reward: itemModel.GetReward());

		itemView.upgradeButtonView.Init(
			increase: cachedIncrease,
			cost: cachedCost,
			costImage: resource.cost["Gold"]);

		return itemView;
	}

	private void SubscribeToUpgradeButton(QuestItemModel itemModel, QuestItemView itemView, BigInteger upgradeCost)
	{
		var gold = CurrencyManager<Gold>.GetCurrency(ECurrencyType.GOLD);

		itemView.upgradeButtonView.button.OnClickAsObservable()
		.Where(_ => gold.CanSubtract(upgradeCost))
		.Subscribe(_ => itemModel.IncreaseLevel())
		.AddTo(itemView.upgradeButtonView.gameObject);

		gold.Subscribe(gold => itemView.upgradeButtonView.SetInteractable(gold >= upgradeCost))
			.AddTo(itemView.upgradeButtonView.gameObject);
	}

	private void SubscribeToQuestItemModel(QuestItemModel itemModel, QuestItemView itemView, QuestTable table)
	{
		itemModel.elpasedTime
			.Subscribe(time =>	itemView.ProgressUpdate(time, table.Time))
			.AddTo(itemView.gameObject);

		itemModel.level.Subscribe(level =>
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