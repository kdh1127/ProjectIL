using UnityEngine;
using Zenject;

public class MainSceneInstaller : MonoInstaller
{
	[SerializeField] private TopPanelView topPanelViewPrefab;
	[SerializeField] private BottomPanelView bottomPanelViewPrefab;
	[SerializeField] private CurrencyView currencyViewPrefab;
	[SerializeField] private MainButtonView mainButtonViewPrefab;

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

		Container.Bind<BottomPanelView>()
			.FromComponentInNewPrefab(bottomPanelViewPrefab)
			.AsSingle();

		Container.Bind<CurrencyView>()
			.FromComponentInNewPrefab(currencyViewPrefab)
			.AsSingle();

		//Container.Bind<MainButtonModel>().AsSingle();
		//Container.Bind<MainButtonView>()
		//	.FromComponentInNewPrefab(mainButtonViewPrefab)
		//	.AsSingle();
	}
}