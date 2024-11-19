using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

public class MissionPresenter
{
	private readonly MissionModel model;
	private readonly MissionPanelView view;
	private readonly MissionItemViewFactory missionItemViewFactory;

	[Inject]
	public MissionPresenter(MissionModel model, MissionPanelView view, MissionItemViewFactory missionItemViewFactory)
	{
		this.model = model;
		this.view = view;
		this.missionItemViewFactory = missionItemViewFactory;
	}

	public void Subscribe()
	{
		var curMissionTable = model.GetCurMissionTable();
		var curMissionItemView = view.currentMissionItemView;

		curMissionItemView.Init(curMissionTable, model.IsClear(curMissionTable));

		curMissionItemView.completeButtonView.button.OnClickAsObservable().Subscribe(_ =>
		{
			model.ClearMission(curMissionTable);
		}).AddTo(curMissionItemView.gameObject);

		model.missionClearSubject.Subscribe(_ =>
		{
			var clearMissionNo = UserDataManager.Instance.missiondata.ClearMissionNo;

			curMissionTable = model.GetCurMissionTable();
			curMissionItemView.UpdateView(curMissionTable, model.IsClear(curMissionTable));
			view.RefreshDisableMissionList(clearMissionNo);
		}).AddTo(view.gameObject);

		view.CreateDisableMissionList(UserDataManager.Instance.missiondata.ClearMissionNo);

		Observable.EveryUpdate().Subscribe(_ =>
		{
			view.currentMissionItemView.UpdateView(curMissionTable, model.IsClear(curMissionTable));
		}).AddTo(view.gameObject);
	}
}
