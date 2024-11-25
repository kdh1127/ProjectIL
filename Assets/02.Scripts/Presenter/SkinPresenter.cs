using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SkinPresenter
{
    private readonly SkinModel model;
    private readonly SkinPanelView view;
    private readonly SkinItemViewFactory factory;
    private readonly CurrencyModel.Dia dia;
    private readonly CharacterView characterView;

    [Inject]
    public SkinPresenter(SkinModel model, SkinPanelView view, SkinItemViewFactory factory, CurrencyModel.Dia dia, CharacterView characterView)
    {
        this.model = model;
        this.view = view;
        this.factory = factory;
        this.dia = dia;
        this.characterView = characterView;
    }

    public void Subscribe()
    {
        var costImageResources = TRScriptableManager.Instance.GetSprite("CostImageResources").spriteDictionary;
        var skinImageResources = TRScriptableManager.Instance.GetSprite("SkinImageResources").spriteDictionary;
        var weaponImageResources = TRScriptableManager.Instance.GetSprite("WeaponImageResources").spriteDictionary;

        SkinTableList.Get().ForEach(table =>
        {
            var itemView = factory.Create();
            var itemModel = model.skinItemList[table.SkinNo];

            var skinImage = skinImageResources[table.SkinImageNo];

            view.RegisterToggle(itemView.equipStatus_tgl);
            itemView.Init(itemModel.m_level.Value, itemModel.GetTotalIncrease(), table.SkinName, table.IncreaseTypeString, itemModel.isEquip.Value, skinImage);
            itemView.OnChangedToggle(view.toggleGroup);
            itemView.upgradeButtonView.Init(itemModel.IsBought, table.UpgradeValue, table.buyCost, table.UpgradeCost, costImageResources["Dia"]);
            itemView.equipStatus_tgl.gameObject.SetActive(itemModel.IsBought);

            itemView.upgradeButtonView.button.OnClickAsObservable()
            .Subscribe(_ =>
            {
                itemModel.IncreaseLevel();
            }).AddTo(itemView.upgradeButtonView.gameObject);
            
            itemModel.m_level.Subscribe(_ =>
            {
                itemView.equipStatus_tgl.gameObject.SetActive(itemModel.IsBought);
                itemView.Init(itemModel.m_level.Value, itemModel.GetTotalIncrease(), table.SkinName, table.IncreaseTypeString, itemModel.isEquip.Value);
                itemView.upgradeButtonView.UpdateView(itemModel.IsBought, table.UpgradeValue, table.buyCost, table.UpgradeCost);
            }).AddTo(itemView.gameObject);

            dia.Subscribe(dia =>
            {
                var cost = itemModel.IsBought ? table.UpgradeCost : table.buyCost;
                itemView.upgradeButtonView.SetInteractable(dia >= cost);
            }).AddTo(itemView.upgradeButtonView.gameObject);

            itemView.equipSkinSubject.Subscribe(isOn => itemModel.UpdateEquipSkin(isOn));

            itemModel.isEquip.Subscribe(isOn =>
            {
                if (isOn)
                {
                    characterView.SetWeapon(skinImage);
                    itemView.equipStatus_tgl.isOn = isOn;
                    itemView.equipStatus_txt.text = isOn ? LocalizationManager.GetTranslation("UnEquip") : LocalizationManager.GetTranslation("Equip");
                }
                else
                {
                    if (model.IsUnEquipAllSkin() == false)
                    {
                        var imageName = WeaponTableList.Get()[model.OriginWeaponNo].Image;
                        var weaponImage = weaponImageResources[imageName];
                        characterView.SetWeapon(weaponImage);
                    }
                }
            }).AddTo(itemView.gameObject);
        });
    }
}


