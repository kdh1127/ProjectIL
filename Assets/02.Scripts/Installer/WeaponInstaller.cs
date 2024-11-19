using UnityEngine;
using Zenject;

public class WeaponInstaller : MonoInstaller
{
	[SerializeField] private WeaponPanelView weaponPanelPrefab;
	[SerializeField] private WeaponItemView weaponItemPrefab;

	public override void InstallBindings()
	{
		WeaponBinding();
	}

	public void WeaponBinding()
	{
		Container.Bind<WeaponModel>().AsSingle();

		Container.Bind<WeaponPanelView>()
			.FromComponentInHierarchy(weaponPanelPrefab)
			.AsSingle();

		Container.Bind<WeaponPresenter>().AsSingle();

		Container.BindFactory<WeaponItemView, WeaponItemViewFactory>()
				 .FromComponentInNewPrefab(weaponItemPrefab)
				 .UnderTransform(weaponPanelPrefab.content_tr)
				 .AsTransient();

	}
}

public class WeaponItemViewFactory : PlaceholderFactory<WeaponItemView> { }