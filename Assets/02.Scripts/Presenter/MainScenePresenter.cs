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
	private StageModel stageModel = new();

	public MainButtonView mainButtonView;
	private MainButtonModel mainButtonModel = new();

	public BottomPanelView bottomPanelView;
	public QuestPanelView questPanelView;
	private QuestModel questModel = new();

	public CurrencyView currencyView;

	private void Awake()
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
		stageModel.CurStage.Subscribe(curStage =>
		{
			var stageFormat = LocalizationManager.GetTranslation("Stage");
			var stageString = string.Format(stageFormat, curStage);
			topPanelView.stage_txt.text = stageString;
		}).AddTo(this);
	}

	public void StageClearTest()
	{
		stageModel.CurStage.Value++;
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
		CurrencyManager.Instance.gold.Subscribe(gold =>
		{
			currencyView.gold_txt.text = gold.ToAlphaString();
		}).AddTo(this);

		CurrencyManager.Instance.dia.Subscribe(dia =>
		{
			currencyView.dia_txt.text = dia.ToAlphaString();
		}).AddTo(this);

		CurrencyManager.Instance.key.Subscribe(key =>
		{
			currencyView.key_txt.text = key.ToAlphaString();
		}).AddTo(this);
	}

	public void QuestPanelSubscribe()
    {
		var questImageResources = TRScriptableManager.Instance.Sprite["QuestImage"].spriteDictionary;
		var currentGold = CurrencyManager.Instance.gold.Value;

		QuestTableList.Get().ForEach(item =>
		{
			// Instantiate
			var itemView = Instantiate(questPanelView.questItem, questPanelView.content_tr).GetComponent<QuestItemView>();

			// Initialize 
			var sprite = questImageResources[item.Image];
			var title = item.Name;
			var increse = new ANumber(item.Increase);
			var cost = new ANumber(item.Cost);

			itemView.Init(sprite, title, increse.ToAlphaString(), cost.ToAlphaString());

			// Subscribe Upgrade_btn
			itemView.quest_btn.OnClickAsObservable().Subscribe(_ =>
			{
				if (currentGold.bigInteger >= cost.bigInteger)
                {
					currentGold -= cost;
				}
			}).AddTo(itemView.gameObject);

			// Update Progress bar in Quest
			var questItem = questModel.questItemList[item.QuestNo];
			var curTime = questItem.elpasedTime;
			var endTime = item.Time;
			var reward = questItem.level * item.Increase;

			Observable.Interval(System.TimeSpan.FromSeconds(1))
			.Subscribe(_ =>
			{
				bool isComplete = itemView.ProgressUpdate(curTime, (int)endTime);
				if (isComplete)
				{
					curTime = 0;
					currentGold += reward;
				}

				curTime++;
			}).AddTo(itemView.gameObject);
		});

	
	}
}
