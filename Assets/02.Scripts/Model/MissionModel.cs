using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MissionItemModel
{

}

public class MissionModel
{
    public void Init()
    {
        MissionTableList.Init(TRScriptableManager.Instance.GetGoogleSheet("MissionTable"));

        // TODO: Load MissionData in UserDataManager
        MissionTableList.Get()
            .ForEach(table =>
            {
                var missionType = EnumList.StringToEnum<EnumList.EMissionType>(table.MissionType);
                switch (missionType)
                {
                    case EnumList.EMissionType.QuestUpgrade:
                        UserDataManager.Instance.missiondata.QuestUpgradeData.Add(table.TargetNo, 0);
                        break;
                    case EnumList.EMissionType.QuestClear:
                        UserDataManager.Instance.missiondata.QuestClearData.Add(table.TargetNo, 0);
                        break;
                    case EnumList.EMissionType.WeaponUpgrade:
                        UserDataManager.Instance.missiondata.WeaponUpgradeData.Add(table.TargetNo, 0);
                        break;
                    case EnumList.EMissionType.DungeonClear:
                        UserDataManager.Instance.missiondata.DungeonClearData.Add(table.TargetNo, 0);
                        break;
                }
            });
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
        var missionType = EnumList.StringToEnum<EnumList.EMissionType>(table.MissionType);

        switch (missionType)
        {
            case EnumList.EMissionType.QuestUpgrade:
                int userAmount = missionData.QuestUpgradeData[table.TargetNo];
                
                if(userAmount >= table.CompleteCount)
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
