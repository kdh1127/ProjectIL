using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Numerics;

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

	public void TreasurePanelSubscribe()
	{
		var costImageResources = TRScriptableManager.Instance.GetSprite("CostImageResources").spriteDictionary;

		TreasureTableList.Get().ForEach(table =>
		{
			var treasureItemView = treasureItemViewFactory.Create();
			var treasureItemModel = model.treasureItemList[table.TreasureNo];

			treasureItemView.Init(table.TreasureName, table.IncreaseType, BigInteger.Parse(table.Increase), treasureItemModel.level.Value);
			treasureItemView.upgradeButtonView.Init(BigInteger.Parse(table.Increase), BigInteger.Parse(table.TreasureCost), costImageResources["Key"]);

		});
	}
}
