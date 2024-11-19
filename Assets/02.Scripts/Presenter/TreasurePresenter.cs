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

	[Inject]
	public TreasurePresenter(TreasureModel model, TreasurePanelView view, TreasureItemViewFactory treasureItemViewFactory)
	{
		this.model = model;
		this.view = view;
		this.treasureItemViewFactory = treasureItemViewFactory;
	}

	public void Subscribe()
	{
		var costImageResources = TRScriptableManager.Instance.GetSprite("CostImageResources").spriteDictionary;

		TreasureTableList.Get().ForEach(table =>
		{
			var treasureItemView = treasureItemViewFactory.Create();
			var treasureItemModel = model.treasureItemList[table.TreasureNo];

			treasureItemView.Init(table.TreasureName, table.IncreaseType, table.Increase.ToBigInt(), treasureItemModel.level.Value);
			treasureItemView.upgradeButtonView.Init(table.Increase.ToBigInt(), table.TreasureCost.ToBigInt(), costImageResources["Key"]);

			//level subscribe
			treasureItemModel.level.Subscribe(level =>
			{
				treasureItemModel.Upgrade();

				treasureItemView.LevelUpdate(level, level * table.Increase.ToBigInt());
				
			}).AddTo(treasureItemView.gameObject);

			//upgrade button subscribe
			treasureItemView.upgradeButtonView.button.OnClickAsObservable()
			.Subscribe(_ =>
			{
				treasureItemModel.level.Value++;
			}).AddTo(treasureItemView.upgradeButtonView.gameObject);

		});

	}
}
