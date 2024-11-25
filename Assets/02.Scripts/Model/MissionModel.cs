using System.Numerics;
using System.Collections.Generic;
using UniRx;
using Zenject;

public class MissionModel
{
    private readonly List<MissionTable> tableList;
    private readonly CurrencyModel.Dia dia;

    [Inject]
    public MissionModel(List<MissionTable> tableList, CurrencyModel.Dia dia)
	{
        this.tableList = tableList;
        this.dia = dia;
	}

    public void Init()
    {
        var missionData = UserDataManager.Instance.missionData;

        // TODO: Load MissionData in UserDataManager
        tableList.ForEach(table =>
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
                    case EMissionType.BuySkin:
                        if (!missionData.BuySkinData.ContainsKey(0))
                            missionData.BuySkinData.Add(0, 0);
                        break;
                }
            });
    }

    public MissionTable GetCurMissionTable()
    {
        var clearMission = UserDataManager.Instance.missionData.ClearMissionNo;
            return tableList[clearMission];
    }

    public void ClearMission()  
    {
        var table = GetCurMissionTable();
        var rewardType = table.RewardType.ToEnum<ECurrencyType>();

        UserDataManager.Instance.missionData.ClearMissionNo++;
        switch (rewardType)
        {
            case ECurrencyType.GOLD:
                //gold.Add(table.Amount.ToBigInt());
                break;
            case ECurrencyType.DIA:
                dia.Add(table.Amount.ToBigInt());
                break;
            case ECurrencyType.KEY:
                //dia.Add(table.Amount.ToBigInt());
                break;
        }
    }

    public int GetCurMissionProgress(MissionTable table)
	{
        var missionData = UserDataManager.Instance.missionData;
        var missionType = table.MissionType.ToEnum<EMissionType>();

		return missionType switch
		{
			EMissionType.QuestUpgrade => missionData.QuestUpgradeData[table.TargetNo],
			EMissionType.QuestClear => missionData.QuestClearData[table.TargetNo],
			EMissionType.WeaponUpgrade => missionData.WeaponUpgradeData[table.TargetNo],
			EMissionType.DungeonClear => missionData.DungeonClearData[table.TargetNo],
            EMissionType.BuySkin => missionData.BuySkinData[0],
			_ => 0
		};
	}

	public bool IsClear(MissionTable table)
    {
        var missionData = UserDataManager.Instance.missionData;
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

                if (userDungeonClearAmount > table.CompleteCount)
                {
                    return true;
                }
                break;
            case EMissionType.BuySkin:
                int userBuySkinAmount = missionData.BuySkinData[0];

                if (userBuySkinAmount >= table.CompleteCount)
				{
                    return true;
				}
                break;
        }

        return false;
    }

}
