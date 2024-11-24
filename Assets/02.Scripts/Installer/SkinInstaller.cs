using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SkinInstaller : MonoInstaller
{
	[SerializeField] private SkinPanelView skinPanelPrefab;
	[SerializeField] private SkinItemView skinItemPrefab;

    public override void InstallBindings()
    {
        SkinBinding();
    }

    public void SkinBinding()
    {
        Container.Bind<SkinModel>().AsSingle();

        Container.Bind<SkinPanelView>()
            .FromComponentInHierarchy(skinPanelPrefab)
            .AsSingle();

        Container.Bind<SkinPresenter>().AsSingle();

        Container.BindFactory<SkinItemView, SkinItemViewFactory>()
            .FromComponentInNewPrefab(skinItemPrefab)
            .UnderTransform(skinPanelPrefab.content_tr)
            .AsTransient();

        Container.Bind<List<SkinTable>>()
            .FromInstance(SkinTableList.Get())
            .AsSingle();
    }
}

public class SkinItemViewFactory : PlaceholderFactory<SkinItemView> { }
