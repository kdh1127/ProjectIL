using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ReincarnationInstaller : MonoInstaller
{
    public OpenPopupButtonView reincarnation_btn;
    public ReincarnationPopupView reincarnationPrefab;
    public override void InstallBindings()
    {
        Container.Bind<ReincarnationPresenter>().AsSingle();
        Container.Bind<ReincarnationModel>().AsSingle();
        Container
            .Bind<OpenPopupButtonView>()
            .FromComponentInHierarchy(reincarnation_btn)
            .AsSingle();
        Container
            .Bind<ReincarnationPopupView>()
            .FromComponentInNewPrefab(reincarnationPrefab)
            .AsTransient();
    }
}
