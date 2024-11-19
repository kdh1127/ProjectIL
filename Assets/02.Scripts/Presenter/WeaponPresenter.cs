using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Numerics;
using Zenject;

public class WeaponPresenter
{
	private readonly WeaponModel model;
	private readonly WeaponPanelView view;
	private readonly WeaponItemViewFactory weaponItemViewFactory;

	[Inject]
	public WeaponPresenter(WeaponModel model, WeaponPanelView view, WeaponItemViewFactory weaponItemViewFactory)
	{
		this.model = model;
		this.view = view;
		this.weaponItemViewFactory = weaponItemViewFactory;
	}

	public void WeaponItemSubscribe(WeaponTable table, WeaponItemView weaponItemView)
	{
		var weaponItemModel = model.weaponItemList[table.WeaponNo];

		weaponItemModel.level.Subscribe(level =>
		{
			model.CheckWeaponState();
		}).AddTo(weaponItemView.gameObject);

		weaponItemModel.UpdateSubject.Subscribe(weaponNo =>
		{
			for (int i = 0; i < view.weaponItemViewList.Count; i++)
			{
				var isEnughGold = CurrencyManager<Gold>.GetCurrency(ECurrencyType.GOLD).Sub(BigInteger.Parse(WeaponTableList.Get()[i].Cost));
				var weaponItemModel = model.weaponItemList[i];
				var table = WeaponTableList.Get()[i];
				view.weaponItemViewList[i].UpdateLevel(
					table: table,
					curLevel: weaponItemModel.level.Value,
					isMaxLevel: weaponItemModel.IsMaxLevel,
					isEquiped: weaponItemModel.isEquiped,
					isUnLock: weaponItemModel.isUnLock,
					isEnughGold: isEnughGold
					);

				if (weaponItemModel.isEquiped)
				{
					weaponItemModel.SetWeaponDamage(table);

				}
			}
		}).AddTo(view.gameObject);
	}

	public void WeaponPanelSubscribe()
	{
		var costImageResources = TRScriptableManager.Instance.GetSprite("CostImageResources").spriteDictionary;

		model.CheckWeaponState();

		WeaponTableList.Get().ForEach(item =>
		{
			var weaponItemView = weaponItemViewFactory.Create();
			var weaponItemModel = model.weaponItemList[item.WeaponNo];
			view.weaponItemViewList.Add(weaponItemView);

			if (item.WeaponNo == 0)
				if (weaponItemModel.level.Value == 0)
				{
					weaponItemModel.level.Value++;

					weaponItemModel.SetWeaponDamage(item);
				}

			weaponItemView.Init(
				table: item,
				curLevel: weaponItemModel.level.Value,
				isMaxLevel: weaponItemModel.IsMaxLevel,
				isEquiped: weaponItemModel.isEquiped,
				isUnLock: weaponItemModel.isUnLock,
				isEnughGold: CurrencyManager<Gold>.GetCurrency(ECurrencyType.GOLD).IsPositive(item.Cost.ToBigInt())
				);

			weaponItemView.upgradeButtonView.Init(
				increase: BigInteger.Parse(item.Increase),
				cost: BigInteger.Parse(item.Cost),
				costImage: costImageResources["Gold"]
				);


			weaponItemView.upgradeButtonView.button.OnClickAsObservable().Subscribe(_ =>
			{
				weaponItemModel.Upgrade(item);
			}).AddTo(weaponItemView.gameObject);

			CurrencyManager<Gold>.GetCurrency(ECurrencyType.GOLD)
			.Subscribe(gold =>
			{
				var isEnughGold = CurrencyManager<Gold>.GetCurrency(ECurrencyType.GOLD).IsPositive(item.Cost.ToBigInt());
				weaponItemView.upgradeButtonView.SetInteractable(isEnughGold);
			}).AddTo(UserDataManager.Instance.gameObject);

			WeaponItemSubscribe(item, weaponItemView);
		});
	}
}
