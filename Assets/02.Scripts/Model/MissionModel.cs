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
    public void Init(List<MissionTable> missionTableList)
    {
        var missionData = UserDataManager.Instance.missiondata;

        // TODO: Load MissionData in UserDataManager
        missionTableList.ForEach(table =>
            {
                var missionType = table.MissionType.ToEnum<EMissionType>();
                switch (missionType)
                {
                    case EMissionType.QuestUpgrade:
                        if(!missionData.QuestUpgradeData.ContainsKey(table.TargetNo))
                            missionData.QuestUpgradeData.Add(table.TargetNo, 0);
                        break;
                    case EMissionType.QuestClear:
                        if (!missionData.QuestClearData.ContainsKey(table.TargetNo))
                            missionData.QuestClearData.Add(table.TargetNo, 0);
                        break;
                    case EMissionType.WeaponUpgrade:
                        if (!missionData.WeaponUpgradeData.ContainsKey(table.TargetNo))
                            missionData.WeaponUpgradeData.Add(table.TargetNo, 0);
                        break;
                    case EMissionType.DungeonClear:
                        if (!missionData.DungeonClearData.ContainsKey(table.TargetNo))
                            missionData.DungeonClearData.Add(table.TargetNo, 0);
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
        var rewardType = table.RewardType.ToEnum<ECurrencyType>();

        UserDataManager.Instance.missiondata.ClearMissionNo++;
        switch (rewardType)
        {
            case ECurrencyType.GOLD:
                CurrencyManager<Gold>.GetCurrency(rewardType).Add(table.Amount.ToBigInt());
                break;
            case ECurrencyType.DIA:
                CurrencyManager<Dia>.GetCurrency(rewardType).Add(table.Amount.ToBigInt());
                break;
            case ECurrencyType.KEY:
                CurrencyManager<Key>.GetCurrency(rewardType).Add(table.Amount.ToBigInt());
                break;
        }
        missionClearSubject.OnNext(Unit.Default);
    }

    public bool IsClear(MissionTable table)
    {
        var missionData = UserDataManager.Instance.missiondata;
        var missionType = table.MissionType.ToEnum<EMissionType>();

        switch (missionType)
        {
            case EMissionType.QuestUpgrade:
                int userQuestUpgradeAmount = missionData.QuestUpgradeData[table.TargetNo];
                
                if(userQuestUpgradeAmount >= table.CompleteCount)
                {
                    return true;
                }
                break;
            case EMissionType.QuestClear:
                int userQuestClearAmount = missionData.QuestClearData[table.TargetNo];

                if (userQuestClearAmount >= table.CompleteCount)
                {
                    return true;
                }
                break;
            case EMissionType.WeaponUpgrade:
                int userWeaponUpgradeAmount = missionData.WeaponUpgradeData[table.TargetNo];

                if (userWeaponUpgradeAmount >= table.CompleteCount)
                {
                    return true;
                }
                break;
            case EMissionType.DungeonClear:
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
