using UnityEngine;
using Zenject;

public class MainSceneInstaller : MonoInstaller
{
	[SerializeField] private TopPanelView topPanelViewPrefab;

	public override void InstallBindings()
	{
		MainSceneBinding();
	}

	public void MainSceneBinding()
	{
		Container.Bind<MainScenePresenter>().AsSingle();

		Container.Bind<TopPanelView>()
			.FromComponentInHierarchy(topPanelViewPrefab)
			.AsSingle();
	}
}