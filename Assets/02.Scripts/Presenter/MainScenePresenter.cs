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
	
	// TODO: ToggleListSubject를 MainButtonModel이 아닌 다른곳에 구현
	// TODO: String에서 Enum이나 딕셔너리 등 좀 더 명확한 값으로 변경
	public void MainButtonSubscribe()
	{
		mainButtonModel.ToggleListSubject(mainButtonView.toggleList);

		mainButtonModel.toggleSubject.Subscribe(tgl =>
		{
			// TODO: 모든 패널 비 활성화
			switch(tgl.name)
			{
				case "Quest_tgl":
					// TODO: 퀘스트 패널 활성화
					break;

				case "Weapon_tgl":
					// TODO: 웨폰 패널 활성화
					break;

				case "Shop_tgl":
					// TODO: 상점 패널 활성화
					break;
			}
		}).AddTo(this);
	}
}
