using System.Collections.Generic;
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

		Container
			.Bind<List<WeaponTable>>()
			.FromInstance(WeaponTableList.Get())
			.AsSingle();

		Container.Bind<WeaponResources>().AsSingle();
	}
}

public class WeaponItemViewFactory : PlaceholderFactory<WeaponItemView> { }
public class WeaponResources
{
	public readonly Dictionary<string, Sprite> weapon;
	public readonly Dictionary<string, Sprite> cost;

	[Inject]
	public WeaponResources(
		[Inject(Id = "WeaponImage")] Dictionary<string, Sprite> weaponImage,
		[Inject(Id = "CostImage")] Dictionary<string, Sprite> costImage)
	{
		this.weapon = weaponImage;
		this.cost = costImage;
	}
}