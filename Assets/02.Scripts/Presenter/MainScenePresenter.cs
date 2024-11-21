using UniRx;
using Zenject;

public class MainScenePresenter
{
	private readonly TopPanelView topPanelView;


	[Inject]
	public MainScenePresenter(TopPanelView topPanelView)
	{
		this.topPanelView = topPanelView;
	}

	public void Subscribe()
	{
		TopPanelSubscribe();
	}

	private void TopPanelSubscribe()
	{
		StageManager.Instance.CurStage.Subscribe(curStage =>
		{
			topPanelView.stage_txt.text = StageManager.Instance.GetLocalizationStage(curStage);
		}).AddTo(topPanelView.gameObject);
	}

}

