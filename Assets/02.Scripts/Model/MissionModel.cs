using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MissionItemModel
{

}

public class MissionModel
{

    public List<MissionItemModel> missionItemList = new();

    public void Init()
    {
        MissionTableList.Init(TRScriptableManager.Instance.GetGoogleSheet("MissionTable"));

        for(int i=0; i < MissionTableList.Get().Count; i++)
        {
            missionItemList.Add(new MissionItemModel());
        }
    }

    /// <summary>
    /// Ŭ���� �� �̼��� ���� �̼��� ���̺� ��ȯ�Ѵ�.
    /// ���� �̼��� ���� ��� ���������� Ŭ���� �� �̼��� ���̺� ��ȯ�Ѵ�.
    /// </summary>
    /// <returns></returns>
    public MissionTable GetCurMissionTable()
    {
        var clearMission = UserDataManager.Instance.missiondata.ClearMissionNo;

        if (clearMission == 0)
            return MissionTableList.Get()[clearMission];
        else if (clearMission < MissionTableList.Get().Count)
            return MissionTableList.Get()[clearMission + 1];
        else
            return MissionTableList.Get()[clearMission];
    }

    public void ClearMission(int missionNo)  
    {
        UserDataManager.Instance.missiondata.ClearMissionNo = missionNo;
    }

    public bool IsClear(MissionTable table)
    {
        var missionData = UserDataManager.Instance.missiondata;
        EnumList.EMissionType missionType = (EnumList.EMissionType)Enum.Parse(typeof(EnumList.EMissionType), table.MissionType);

        switch (missionType)
        {
            case EnumList.EMissionType.QuestUpgrade:
                int userAmount = missionData.QuestUpgradeData[table.TargetNo];

                if(userAmount > table.CompleteCount)
                {
                    return true;
                }
                break;
            case EnumList.EMissionType.QuestClear:
                break;
            case EnumList.EMissionType.WeaponUpgrade:
                break;
            case EnumList.EMissionType.DungeonClear:
                break;
                
        }

        return false;
    }

}
