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

		TreasureTableList.Get().ForEach(table =>
		{
			var treasureItemView = treasureItemViewFactory.Create();
			var treasureItemModel = model.treasureItemList[table.TreasureNo];

			treasureItemView.Init(table.TreasureName, table.IncreaseTypeString, treasureItemModel.level.Value, table.Increase.ToBigInt());
			treasureItemView.upgradeButtonView.Init(table.Increase.ToBigInt(), table.TreasureCost.ToBigInt(), costImageResources["Key"]);

			//upgradeButtonView.button.OnClickAsObservable()
			//.Where(_ => gold.CanSubtract(upgradeCost))
			//.Subscribe(_ => itemModel.IncreaseLevel())
			//.AddTo(upgradeButtonView.gameObject);

			//level subscribe
			treasureItemModel.level.Subscribe(level =>
			{
				treasureItemView.LevelUpdate(level, level * table.Increase.ToBigInt());
			}).AddTo(treasureItemView.gameObject);

			//upgrade button subscribe
			treasureItemView.upgradeButtonView.button.OnClickAsObservable()
			.Subscribe(_ =>
			{
				treasureItemModel.Upgrade();
			}).AddTo(treasureItemView.upgradeButtonView.gameObject);

			key.Subscribe(key => treasureItemView.upgradeButtonView.SetInteractable(key >= table.TreasureCost.ToBigInt()))
				.AddTo(treasureItemView.upgradeButtonView.gameObject);
		});

	}
}
