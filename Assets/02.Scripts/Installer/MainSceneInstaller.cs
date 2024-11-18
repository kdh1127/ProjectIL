using Zenject;

public class MainSceneInstaller : MonoInstaller
{
	public QuestPanelView questPanelPrefab;
	public QuestItemView questItemPrefab;

	public override void InstallBindings()
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
	}
}

public class QuestItemViewFactory : PlaceholderFactory<QuestItemView> { }