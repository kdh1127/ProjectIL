using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

public class MissionPresenter
{
	private readonly MissionModel model;
	private readonly MissionItemView itemView;

	[Inject]
	public MissionPresenter(MissionModel model, MissionItemView itemView)
	{
		this.model = model;
		this.itemView = itemView;
	}

	public void Subscribe()
	{
		InitItemView(model, itemView);
		InitCompleteButton(model, itemView.completeButtonView);
		EveryUpdateItemView(model, itemView);
	}

	private void InitItemView(MissionModel model, MissionItemView itemView)
	{
		var curMissionTable = model.GetCurMissionTable();
		itemView.Init(curMissionTable, model.IsClear(curMissionTable), model.GetCurMissionProgress(curMissionTable));
	}

	private void InitCompleteButton(MissionModel model, CompleteButtonView completeButtonView)
	{
		var curMissionTable = model.GetCurMissionTable();
		completeButtonView.button
			.OnClickAsObservable()
			.Subscribe(_ => model.ClearMission())
			.AddTo(completeButtonView.gameObject);
	}

	private void EveryUpdateItemView(MissionModel model, MissionItemView itemView)
	{
		Observable
			.EveryUpdate()
			.Subscribe(_ => UpdateItemView(model, itemView))
			.AddTo(itemView.gameObject);
	}

	private void UpdateItemView(MissionModel model, MissionItemView itemView)
	{
		var curMissionTable = model.GetCurMissionTable();
		itemView.UpdateView(curMissionTable, model.IsClear(curMissionTable), model.GetCurMissionProgress(curMissionTable));
	}

}
