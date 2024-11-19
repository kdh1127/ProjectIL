using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MissionInstaller : MonoInstaller
{
	[SerializeField] private MissionPanelView missionPanelPrefab;
	[SerializeField] private MissionItemView missionItemPrefab;

	public override void InstallBindings()
	{
		MissionBinding();
	}

	public void MissionBinding()
	{
		Container.Bind<MissionModel>().AsSingle();

		Container.Bind<MissionPanelView>()
			.FromComponentInHierarchy(missionPanelPrefab)
			.AsSingle();

		Container.Bind<MissionPresenter>().AsSingle();

		Container.BindFactory<MissionItemView, MissionItemViewFactory>()
				 .FromComponentInNewPrefab(missionItemPrefab)
				 .UnderTransform(missionPanelPrefab.content_tr)
				 .AsTransient();
	}
}

public class MissionItemViewFactory : PlaceholderFactory<MissionItemView> { }