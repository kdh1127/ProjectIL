using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using UniRx;

public class MissionModel
{
    public Subject<Unit> missionClearSubject = new();
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
            return MissionTableList.Get()[clearMission];
    }

    public void ClearMission(MissionTable table)  
    {
        var rewardType = EnumList.StringToEnum<EnumList.ECurrencyType>(table.RewardType);

        UserDataManager.Instance.missiondata.ClearMissionNo++;
        UserDataManager.Instance.currencyData.Currency[rewardType].Value += BigInteger.Parse(table.Amount);
        missionClearSubject.OnNext(Unit.Default);
    }

    public bool IsClear(MissionTable table)
    {
        var missionData = UserDataManager.Instance.missiondata;
        var missionType = EnumList.StringToEnum<EnumList.EMissionType>(table.MissionType);

        switch (missionType)
        {
            case EnumList.EMissionType.QuestUpgrade:
                int userQuestUpgradeAmount = missionData.QuestUpgradeData[table.TargetNo];
                
                if(userQuestUpgradeAmount >= table.CompleteCount)
                {
                    return true;
                }
                break;
            case EnumList.EMissionType.QuestClear:
                int userQuestClearAmount = missionData.QuestClearData[table.TargetNo];

                if (userQuestClearAmount >= table.CompleteCount)
                {
                    return true;
                }
                break;
            case EnumList.EMissionType.WeaponUpgrade:
                int userWeaponUpgradeAmount = missionData.WeaponUpgradeData[table.TargetNo];

                if (userWeaponUpgradeAmount >= table.CompleteCount)
                {
                    return true;
                }
                break;
            case EnumList.EMissionType.DungeonClear:
                int userDungeonClearAmount = missionData.DungeonClearData[table.TargetNo];

                if (userDungeonClearAmount >= table.CompleteCount)
                {
                    return true;
                }
                break;
        }

        return false;
    }

}
