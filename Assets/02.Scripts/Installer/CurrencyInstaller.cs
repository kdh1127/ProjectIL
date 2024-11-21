using Zenject;

public class CurrencyInstaller : MonoInstaller
{
	public CurrencyView currencyViewPrefab;

	public override void InstallBindings()
	{
		Container.Bind<CurrencyModel.Gold>().AsSingle();
		Container.Bind<CurrencyModel.Dia>().AsSingle();
		Container.Bind<CurrencyModel.Key>().AsSingle();

		Container.Bind<CurrencyPresenter>().AsSingle();
		Container.Bind<CurrencyModel>().AsSingle();
		Container
			.Bind<CurrencyView>()
			.FromComponentInHierarchy(currencyViewPrefab)
			.AsCached();
	}
}
