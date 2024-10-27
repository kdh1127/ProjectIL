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
			var stageFormat = "{0} Stage";//LocalizationManager.GetTermTranslation("Stage");
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

	public void QuestItemSubscribe(QuestTable item, QuestItemView questItemView)
    {
        var questItem = questModel.questItemList[item.QuestNo];

		questItem.elpasedTime.Subscribe(time =>
		{
			var currentGold = CurrencyManager.Instance.gold;
			bool isComplete = questItemView.ProgressUpdate(time, (int)item.Time);
			if (isComplete)
			{
				questItem.elpasedTime.Value = 0;
				currentGold.Value += questItem.level.Value * item.Increase;
			}


		}).AddTo(questItemView.gameObject);

		questItem.level.Subscribe(level =>
		{
			var reward = questItem.level.Value > 0 ? questItem.level.Value * item.Increase : item.Increase;

			questItemView.level_txt.text = $"Lv.{level}";
			questItemView.reward_txt.text = reward.ToString();
		}).AddTo(questItemView.gameObject);
    }

	public void QuestPanelSubscribe()
    {
		var questImageResources = TRScriptableManager.Instance.Sprite["QuestImage"].spriteDictionary;
		var costImageResources = TRScriptableManager.Instance.Sprite["CostImage"].spriteDictionary;
		var currentGold = CurrencyManager.Instance.gold;

		QuestTableList.Get().ForEach(item =>
		{
			// Instantiate
			var itemView = Instantiate(questPanelView.questItem, questPanelView.content_tr).GetComponent<QuestItemView>();
			var questItem = questModel.questItemList[item.QuestNo];
			
			// Initialize 
			itemView.Init(
				sprite: questImageResources[item.Image],
				title: item.Name,
				endTime: item.Time,
				level: questItem.level.Value,
				reward: questItem.level.Value > 0 ? questItem.level.Value * item.Increase : item.Increase);

			var cost = new ANumber(item.Cost);
			itemView.upgradeButtonView.Init(new ANumber(item.Increase).ToAlphaString(), cost.ToAlphaString(), costImageResources["Gold"]);

			// Subscribe QuestItemModel
			QuestItemSubscribe(item, itemView);

			// Subscribe Upgrade_btn
			itemView.upgradeButtonView.button.OnClickAsObservable().Subscribe(_ =>
			{
				if (currentGold.Value.bigInteger >= cost.bigInteger)
                {
					currentGold.Value -= cost;
					questModel.questItemList[item.QuestNo].level.Value++;
				}
			}).AddTo(itemView.gameObject);

			// Subscribe currentGold
			currentGold.Subscribe(gold =>
			{
				itemView.upgradeButtonView.SetInteractable(gold.bigInteger >= cost.bigInteger);
			}).AddTo(itemView.upgradeButtonView.button);

			// Update Progress bar in Quest
			Observable.Interval(System.TimeSpan.FromSeconds(1))
			.Where(_ => questItem.level.Value > 0)
			.Subscribe(_ =>
			{
				questItem.elpasedTime.Value++;
			}).AddTo(itemView.gameObject);
		});
	}
}
