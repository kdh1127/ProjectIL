using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using ThreeRabbitPackage.DesignPattern;
using I2.Loc;
using AlphabetNumber;
using System.Numerics;

public class MainScenePresenter : TRSingleton<MainScenePresenter>
{
	public TopPanelView topPanelView;

	public MainButtonView mainButtonView;
	private MainButtonModel mainButtonModel = new();

	public BottomPanelView bottomPanelView;
	public QuestPanelView questPanelView;
	private QuestModel questModel = new();

	public CurrencyView currencyView;

	private new void Awake()
	{
		base.Awake();

		questModel.Init();

		TopPanelSubscribe();
		MainButtonSubscribe();
		CurrencySubscribe();
		QuestPanelSubscribe();
	}

	private void TopPanelSubscribe()
	{
		StageManager.Instance.CurStage.Subscribe(curStage =>
		{
			topPanelView.stage_txt.text = StageManager.Instance.GetLocalizationStage(curStage);
		}).AddTo(this);
	}

	public void MainButtonSubscribe()
	{
		mainButtonModel.RegisterToggleList(mainButtonView.toggleGroup, mainButtonView.toggleList);
		mainButtonModel.toggleSubject.Subscribe(tgl =>
		{
			mainButtonView.DeactivateAllPanel(bottomPanelView.panelList);
			mainButtonView.ActivatePanel(tgl.type);
		}).AddTo(this);
	}

	public void CurrencySubscribe()
	{
		UserDataManager.Instance.Currency[EnumList.ECurrencyType.GOLD].Subscribe(gold =>
		{
			currencyView.gold_txt.text = gold.ToAlphabetNumber();
		}).AddTo(this);

		UserDataManager.Instance.Currency[EnumList.ECurrencyType.DIA].Subscribe(dia =>
		{
			currencyView.dia_txt.text = dia.ToAlphabetNumber();
		}).AddTo(this);

		UserDataManager.Instance.Currency[EnumList.ECurrencyType.KEY].Subscribe(key =>
		{
			currencyView.key_txt.text = key.ToAlphabetNumber();
		}).AddTo(this);
	}

	#region Quest
	public void QuestItemSubscribe(QuestTable table, QuestItemView questItemView)
    {
        var questItemModel = questModel.questItemList[table.QuestNo];

		questItemModel.elpasedTime.Subscribe(time =>
		{
			questItemView.ProgressUpdate(time, (int)table.Time);
		}).AddTo(questItemView.gameObject);

		questItemModel.level.Subscribe(level =>
		{
			questItemView.LevelUpdate(level.ToString(), questItemModel.GetReward(table).ToString());
		}).AddTo(questItemView.gameObject);
    }

	public void QuestPanelSubscribe()
    {
		var questImageResources = TRScriptableManager.Instance.Sprite["QuestImage"].spriteDictionary;
		var costImageResources = TRScriptableManager.Instance.Sprite["CostImage"].spriteDictionary;

		QuestTableList.Get().ForEach(item =>
		{
			// Instantiate
			var qusetitemView = Instantiate(questPanelView.questItem, questPanelView.content_tr).GetComponent<QuestItemView>();
			var questItemModel = questModel.questItemList[item.QuestNo];

			// Initialize 
			qusetitemView.Init(
				sprite: questImageResources[item.Image],
				title: item.Name,
				endTime: item.Time,
				level: questItemModel.level.Value,
				reward: questItemModel.GetReward(item));

			qusetitemView.upgradeButtonView.Init(
				increase: new ANumber(item.Increase).ToAlphaString(),
				cost: new ANumber(item.Cost).ToAlphaString(),
				costImage: costImageResources["Gold"]);

			// Subscribe QuestItemModel
			QuestItemSubscribe(item, qusetitemView);

			// Subscribe Upgrade_btn
			qusetitemView.upgradeButtonView.button.OnClickAsObservable().Subscribe(_ =>
			{
				questModel.questItemList[item.QuestNo].Upgrade(item);
			}).AddTo(qusetitemView.gameObject);

			// Subscribe currentGold
			UserDataManager.Instance.Currency[EnumList.ECurrencyType.GOLD].Subscribe(gold =>
			{
				qusetitemView.upgradeButtonView.SetInteractable(gold >= item.Cost);
			}).AddTo(qusetitemView.upgradeButtonView.button);

			// Update Progress bar in Quest
			Observable.Interval(System.TimeSpan.FromSeconds(1))
			.Where(_ => questItemModel.IsOn)
			.Subscribe(_ =>
			{
				questItemModel.Progress(item);
			}).AddTo(qusetitemView.gameObject);
		});
	}
	#endregion
}
