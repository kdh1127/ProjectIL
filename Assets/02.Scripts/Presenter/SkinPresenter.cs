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
    public SkinPresenter (SkinModel model, SkinPanelView view, SkinItemViewFactory factory, CurrencyModel.Dia dia, CharacterView characterView)
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


        SkinTableList.Get().ForEach(table =>
        {
            var itemView = factory.Create();
            var itemModel = model.skinItemList[table.SkinNo];

            var skinImageResources = TRScriptableManager.Instance.GetSprite("SkinImageResources").spriteDictionary;
            var skinImage = skinImageResources[table.SkinImageNo];

            view.RegisterToggle(itemView.equipStatus_tgl);
            itemView.Init(itemModel.m_level.Value, itemModel.GetTotalIncrease(), table.SkinName, skinImage);
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
                itemView.Init(itemModel.m_level.Value, itemModel.GetTotalIncrease());
                itemView.upgradeButtonView.UpdateView(itemModel.IsBought, table.UpgradeValue, table.buyCost, table.UpgradeCost);
            }).AddTo(itemView.gameObject);


            dia.Subscribe(dia =>
            {
                var cost = itemModel.IsBought ? table.UpgradeCost : table.buyCost;
                itemView.upgradeButtonView.SetInteractable(dia >= cost);
            }).AddTo(itemView.upgradeButtonView.gameObject);

            itemView.equipSkinSubject.Subscribe(isOn =>
            {
                if (isOn)
                {
                    characterView.SetWeapon(skinImage);
                }
            }).AddTo(itemView.gameObject);
        });


    }
}
