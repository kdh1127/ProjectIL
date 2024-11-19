using UnityEngine;
using Zenject;

public class MainSceneInstaller : MonoInstaller
{
	[SerializeField] private TopPanelView topPanelViewPrefab;
	[SerializeField] private CurrencyView currencyViewPrefab;

	public override void InstallBindings()
	{
		MainSceneBinding();
	}

	public void MainSceneBinding()
	{
		Container.Bind<MainScenePresenter>().AsSingle();

		Container.Bind<TopPanelView>()
			.FromComponentInNewPrefab(topPanelViewPrefab)
			.AsSingle();

		Container.Bind<CurrencyView>()
			.FromComponentInHierarchy(currencyViewPrefab)
			.AsSingle();
	}
}