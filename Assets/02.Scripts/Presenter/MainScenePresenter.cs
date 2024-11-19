using UniRx;
using Zenject;
using ThreeRabbitPackage.DesignPattern;

public class MainScenePresenter
{
	private readonly TopPanelView topPanelView;
	private readonly BottomPanelView bottomPanelView;
	private readonly CurrencyView currencyView;


	[Inject]
	public MainScenePresenter(
		TopPanelView topPanelView,
		BottomPanelView bottomPanelView,
		CurrencyView currencyView
		)
	{
		this.topPanelView = topPanelView;
		this.bottomPanelView = bottomPanelView;
		this.currencyView = currencyView;
	}

	public void Subscribe()
	{
		TopPanelSubscribe();
		CurrencySubscribe();
	}

	private void TopPanelSubscribe()
	{
		StageManager.Instance.CurStage.Subscribe(curStage =>
		{
			topPanelView.stage_txt.text = StageManager.Instance.GetLocalizationStage(curStage);
		}).AddTo(topPanelView.gameObject);
	}

	public void CurrencySubscribe()
	{
		CurrencyManager<Gold>.GetCurrency(ECurrencyType.GOLD).Subscribe(gold =>
		{
			currencyView.gold_txt.text = gold.ToAlphabetNumber();
		}).AddTo(currencyView.gameObject);

		CurrencyManager<Dia>.GetCurrency(ECurrencyType.DIA).Subscribe(dia =>
		{
			currencyView.dia_txt.text = dia.ToAlphabetNumber();
		}).AddTo(currencyView.gameObject);

		CurrencyManager<Key>.GetCurrency(ECurrencyType.KEY).Subscribe(key =>
		{
			currencyView.key_txt.text = key.ToAlphabetNumber();
		}).AddTo(currencyView.gameObject);
	}
}

