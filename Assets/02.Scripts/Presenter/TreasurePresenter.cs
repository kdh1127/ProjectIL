using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Numerics;
using UniRx;

public class TreasurePresenter
{
	private readonly TreasureModel model;
	private readonly TreasurePanelView view;
	private readonly TreasureItemViewFactory treasureItemViewFactory;
	private readonly CurrencyModel.Key key;


	[Inject]
	public TreasurePresenter(TreasureModel model, TreasurePanelView view, TreasureItemViewFactory treasureItemViewFactory, CurrencyModel.Key key)
	{
		this.model = model;
		this.view = view;
		this.treasureItemViewFactory = treasureItemViewFactory;
		this.key = key;
	}

	public void Subscribe()
	{
		var costImageResources = TRScriptableManager.Instance.GetSprite("CostImageResources").spriteDictionary;
		var treasureImageResources = TRScriptableManager.Instance.GetSprite("TreasureImageResources").spriteDictionary;
		TreasureTableList.Get().ForEach(table =>
		{
			var treasureItemView = treasureItemViewFactory.Create();
			var treasureItemModel = model.treasureItemList[table.TreasureNo];

			treasureItemView.Init(treasureImageResources[table.IncreaseType], table.TreasureName, table.IncreaseTypeString, treasureItemModel.level.Value, table.Increase.ToBigInt());
			treasureItemView.upgradeButtonView.Init(table.Increase.ToBigInt(), table.TreasureCost.ToBigInt(), costImageResources["Key"]);

			//level subscribe
			treasureItemModel.level.Subscribe(level =>
			{
				treasureItemView.LevelUpdate(level, table.TreasureName, level * table.Increase.ToBigInt());
				treasureItemView.upgradeButtonView.UpdateView(table.Increase.ToBigInt().ToAlphabetNumber(), treasureItemModel.GetCost().ToAlphabetNumber());
			}).AddTo(treasureItemView.gameObject);

			//upgrade button subscribe
			treasureItemView.upgradeButtonView.button.OnClickAsObservable()
			.Subscribe(_ =>
			{
				treasureItemModel.Upgrade();
			}).AddTo(treasureItemView.upgradeButtonView.gameObject);

			key.Subscribe(key => treasureItemView.upgradeButtonView.SetInteractable(key >= treasureItemModel.GetCost()))
				.AddTo(treasureItemView.upgradeButtonView.gameObject);
		});

	}
}
