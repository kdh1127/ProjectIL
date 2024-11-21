using System.Numerics;
using System.Collections.Generic;
using UniRx;
using Zenject;

public class MissionModel
{
    private readonly List<MissionTable> tableList;
    private readonly CurrencyModel.Gold gold;

    [Inject]
    public MissionModel(List<MissionTable> tableList, CurrencyModel.Gold gold)
	{
        this.tableList = tableList;
        this.gold = gold;
	}

    public void Init()
    {
        var missionData = UserDataManager.Instance.missiondata;

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
                }
            });
    }

    public MissionTable GetCurMissionTable()
    {
        var clearMission = UserDataManager.Instance.missiondata.ClearMissionNo;
            return tableList[clearMission];
    }

    public void ClearMission(MissionTable table)  
    {
        var rewardType = table.RewardType.ToEnum<ECurrencyType>();

        UserDataManager.Instance.missiondata.ClearMissionNo++;
        switch (rewardType)
        {
            case ECurrencyType.GOLD:
                gold.Add(table.Amount.ToBigInt());
                break;
            case ECurrencyType.DIA:
                gold.Add(table.Amount.ToBigInt());
                break;
            case ECurrencyType.KEY:
                gold.Add(table.Amount.ToBigInt());
                break;
        }
    }

    public int GetCurMissionProgress(MissionTable table)
	{
        var missionData = UserDataManager.Instance.missiondata;
        var missionType = table.MissionType.ToEnum<EMissionType>();

		return missionType switch
		{
			EMissionType.QuestUpgrade => missionData.QuestUpgradeData[table.TargetNo],
			EMissionType.QuestClear => missionData.QuestClearData[table.TargetNo],
			EMissionType.WeaponUpgrade => missionData.WeaponUpgradeData[table.TargetNo],
			EMissionType.DungeonClear => missionData.DungeonClearData[table.TargetNo],
			_ => 0
		};
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
