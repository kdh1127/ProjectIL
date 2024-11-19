using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class QuestInstaller : MonoInstaller
{
	[SerializeField] private QuestPanelView questPanelPrefab;
	[SerializeField] private QuestItemView questItemPrefab;

    public override void InstallBindings()
    {
		QuestBinding();
    }

    public void QuestBinding()
	{
		Container.Bind<QuestModel>().AsSingle();

		Container.Bind<QuestPanelView>()
			.FromComponentInNewPrefab(questPanelPrefab)
			.AsSingle();


		Container.Bind<QuestPresenter>().AsSingle();

		Container.BindFactory<QuestItemView, QuestItemViewFactory>()
				 .FromComponentInNewPrefab(questItemPrefab)
				 .UnderTransform(questPanelPrefab.content_tr)
				 .AsTransient();
	}
}

public class QuestItemViewFactory : PlaceholderFactory<QuestItemView> { }