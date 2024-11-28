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
	private readonly CurrencyModel.Gold gold;
	private readonly CharacterView characterView;
	private readonly SkinModel skinModel;

	[Inject]
	public WeaponPresenter(
		WeaponModel model,
		WeaponPanelView view,
		CharacterView characterView,
		WeaponItemViewFactory weaponItemViewFactory,
		CurrencyModel.Gold gold,
		SkinModel skinModel)
	{
		this.model = model;
		this.view = view;
		this.characterView = characterView;
		this.weaponItemViewFactory = weaponItemViewFactory;
		this.gold = gold;
		this.skinModel = skinModel;
	}

	public void Subscribe()
	{
		WeaponTableList.Get().ForEach(table =>
		{
			var weaponItemView = weaponItemViewFactory.Create();
			var weaponItemModel = model.weaponItemList[table.WeaponNo];

			weaponItemModel.UpdateState();
			InitItemView(weaponItemModel, weaponItemView, table);
			SubscribeToWeaponItemModel(weaponItemModel, weaponItemView, table);
			SubscribeToUpgradeButton(weaponItemModel, weaponItemView.upgradeButtonView);
			SubscribeToGold(weaponItemModel, weaponItemView.upgradeButtonView, table);
		});
	}

	public void InitItemView(WeaponItemModel itemModel, WeaponItemView itemView, WeaponTable table)
	{
		var costImageResources = TRScriptableManager.Instance.GetSprite("CostImageResources").spriteDictionary;

		itemView.Init(
			table: table,
			curLevel: itemModel.m_level.Value,
			isMaxLevel: itemModel.isMaxLevel.Value,
			isEquiped: itemModel.isEquiped.Value,
			isUnLock: itemModel.isUnLock.Value,
			isEnughGold: gold.CanSubtract(table.Cost.ToBigInt()));

		itemView.upgradeButtonView.Init(
			increase: table.Increase.ToBigInt(),
			cost: table.Cost.ToBigInt(),
			costImage: costImageResources["Gold"]);

		UpdateItemView(itemModel, itemView, table);
	}
	public void UpdateItemView(WeaponItemModel itemModel, WeaponItemView itemView, WeaponTable table)
	{
		var isEnughGold = gold.CanSubtract(table.Cost.ToBigInt());

		itemView.UpdateLevel(
			table: table,
			curLevel: itemModel.m_level.Value,
			isMaxLevel: itemModel.isMaxLevel.Value,
			isEquiped: itemModel.isEquiped.Value,
			isUnLock: itemModel.isUnLock.Value,
			isEnughGold: isEnughGold);
	}
	public void SubscribeToWeaponItemModel(WeaponItemModel itemModel, WeaponItemView itemView, WeaponTable table)
	{
		itemModel.m_level.Subscribe(_ => UpdateItemView(itemModel, itemView, table)).AddTo(itemView.gameObject);
		itemModel.isUnLock.Subscribe(_ => UpdateItemView(itemModel, itemView, table)).AddTo(itemView.gameObject);
		itemModel.isMaxLevel.Subscribe(_ => UpdateItemView(itemModel, itemView, table)).AddTo(itemView.gameObject);
		itemModel.isEquiped.Subscribe(isEquip =>
		{
			UpdateItemView(itemModel, itemView, table);

			if (isEquip)
			{
				var weaponImageResources = TRScriptableManager.Instance.GetSprite("WeaponImageResources").spriteDictionary;
				itemModel.SetWeaponDamage();
				if (skinModel.IsUnEquipAllSkin() == false)
				{
					var originWeaponNo = UserDataManager.Instance.skinData.originWeaponNo;
					var weaponImage = weaponImageResources[WeaponTableList.Get()[originWeaponNo].Image];
					characterView.SetWeapon(weaponImage);
				}
			}
		}).AddTo(itemView.gameObject);
	}
	public void SubscribeToUpgradeButton(WeaponItemModel itemModel, UpgradeButtonView upgradeButtonView)
	{
		upgradeButtonView.button.OnClickAsObservable().Subscribe(_ =>
		{
			itemModel.Upgrade();
			if (skinModel.IsUnEquipAllSkin() == false)
			{
				var weaponImageResources = TRScriptableManager.Instance.GetSprite("WeaponImageResources").spriteDictionary;
				var originWeaponNo = UserDataManager.Instance.skinData.originWeaponNo;
				var weaponImage = weaponImageResources[WeaponTableList.Get()[originWeaponNo].Image];
				characterView.SetWeapon(weaponImage);
			}
		}).AddTo(upgradeButtonView.gameObject);
	}
	public void SubscribeToGold(WeaponItemModel itemModel, UpgradeButtonView upgradeButtonView, WeaponTable table)
	{
		gold
		.Where(_ => itemModel.isUnLock.Value)
		.Subscribe(_ =>
		{
			var isEnughGold = gold.CanSubtract(table.Cost.ToBigInt());
			upgradeButtonView.SetInteractable(isEnughGold);
		}).AddTo(upgradeButtonView.gameObject);
	}
}
