using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using ThreeRabbitPackage.DesignPattern;
using I2.Loc;

public class MainScenePresenter : TRSingleton<MainScenePresenter>
{
	public StageView stageView;
	private StageModel stageModel = new();

	public MainButtonView mainButtonView;
	private MainButtonModel mainButtonModel = new();

	public BottomPanelView bottomPanelView;
	
	private void Awake()
	{
		MainButtonSubscribe();
		StageSubscribe();
	}

	private void StageSubscribe()
	{
		stageModel.CurStage.Subscribe(curStage =>
		{
			var stageFormat = LocalizationManager.GetTranslation("Stage");
			var stageString = string.Format(stageFormat, curStage);
			stageView.stage_txt.text = stageString;
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
}
