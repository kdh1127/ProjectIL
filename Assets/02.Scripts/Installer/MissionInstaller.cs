using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MissionInstaller : MonoInstaller
{
	[SerializeField] private MissionItemView missionItemPrefab;

	public override void InstallBindings()
	{
		MissionBinding();
	}

	public void MissionBinding()
	{
		Container.Bind<MissionModel>().AsSingle();

		Container.Bind<MissionPresenter>().AsSingle();

		Container
			.Bind<MissionItemView>()
			.FromComponentInHierarchy(missionItemPrefab)
			.AsSingle();

		Container
			.Bind<List<MissionTable>>()
			.FromInstance(MissionTableList.Get())
			.AsSingle();
	}
}