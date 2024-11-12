using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionPanelView : MonoBehaviour
{
    public MissionItemView currentMissionItemView;
    public Transform content_tr;
    public GameObject missionItem;

	public void CreateDisableMissionList(int clearMissionNo)
    {
		for (int i = clearMissionNo + 1; i < MissionTableList.Get().Count; i++)
		{
			var missionItemView = Instantiate(missionItem, content_tr).GetComponent<MissionItemView>();
			missionItemView.Init(MissionTableList.Get()[i]);
		}
	}
    //TODO: refactoring List to Queue & Method Extension
    public void RefreshDisableMissionList(int clearMissionNo)
	{
		for (int i = 0; i < content_tr.childCount; i++)
		{
			Destroy(content_tr.GetChild(i).gameObject);
		}

		for (int i = clearMissionNo + 1; i < MissionTableList.Get().Count; i++)
		{
			var missionItemView = Instantiate(missionItem, content_tr).GetComponent<MissionItemView>();
			missionItemView.Init(MissionTableList.Get()[i]);
		}
	}
}
