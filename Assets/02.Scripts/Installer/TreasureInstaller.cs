using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


public class TreasureInstaller : MonoInstaller
{
	[SerializeField] private TreasurePanelView treasurePanelPrefab;
	[SerializeField] private TreasureItemView treasureItemPrefab;

	public override void InstallBindings()
	{
		TreasureBinding();
	}

	public void TreasureBinding()
	{
		Container.Bind<TreasureModel>().AsSingle();

		Container.Bind<TreasurePanelView>()
			.FromComponentInHierarchy(treasurePanelPrefab)
			.AsSingle();

		Container.Bind<TreasurePresenter>().AsSingle();

		Container.BindFactory<TreasureItemView, TreasureItemViewFactory>()
				 .FromComponentInNewPrefab(treasureItemPrefab)
				 .UnderTransform(treasurePanelPrefab.content_tr)
				 .AsTransient();

		Container
			.Bind<List<TreasureTable>>()
			.FromInstance(TreasureTableList.Get())
			.AsSingle();
	}
}

public class TreasureItemViewFactory : PlaceholderFactory<TreasureItemView> { }