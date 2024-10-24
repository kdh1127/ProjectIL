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
		questModel.Init();
		QuestTableList.Get().ForEach(item =>
		{
			var questItem = Instantiate(questPanelView.questItem, questPanelView.content_tr);

			ANumber increse = new (item.Increase);
			ANumber cost = new (item.Cost);

			var sprite = questModel.questImageResources.spriteDictionary[item.Image];
			questItem.GetComponent<QuestItemView>().Init(sprite, item.Name, increse.ToAlphaString(), cost.ToAlphaString());

			questItem.GetComponent<QuestItemView>().quest_btn.OnClickAsObservable().Subscribe(_ =>
			{
				var myGold = CurrencyManager.Instance.gold.Value;

				if (myGold.bigInteger >= cost.bigInteger)
                {
					CurrencyManager.Instance.gold.Value -= cost;
					// TODO: 퀘스트 인크리즈값 * 레벨 = 내가 버는 재화를 해당 타이머가 완료되면 벌 수 있도록 해야하고
					// 타이머의 정보도 저장이 되어야 함
				}

			}).AddTo(this.gameObject);
		});

	
	}
}
