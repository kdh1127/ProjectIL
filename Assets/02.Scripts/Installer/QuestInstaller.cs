using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using ThreeRabbitPackage;

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
			.FromComponentInHierarchy(questPanelPrefab)
			.AsSingle();


		Container.Bind<QuestPresenter>().AsSingle();

		Container.BindFactory<QuestItemView, QuestItemViewFactory>()
				 .FromComponentInNewPrefab(questItemPrefab)
				 .UnderTransform(questPanelPrefab.content_tr)
				 .AsTransient();

		Container.Bind<QuestResources>().AsSingle();
	}
}

public class QuestItemViewFactory : PlaceholderFactory<QuestItemView> { }
public class QuestResources
{
	public readonly Dictionary<string, Sprite> quest;
	public readonly Dictionary<string, Sprite> cost;

	[Inject]
	public QuestResources(
		[Inject(Id = "QuestImage")] Dictionary<string, Sprite> questImage,
		[Inject(Id = "CostImage")] Dictionary<string, Sprite> costImage)
	{
		this.quest = questImage;
		this.cost = costImage;
	}
}