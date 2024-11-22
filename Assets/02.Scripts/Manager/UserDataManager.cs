using System.Numerics;
using System.Collections.Generic;
using ThreeRabbitPackage.DesignPattern;
using UniRx;
using UnityEngine;
using System;

public class UserDataManager : TRSingleton<UserDataManager>
{
	public CharacterData characterData = new();
	public MissionData missiondata = new();
	private bool isInit = false;
	private new void Awake()
	{
		base.Awake();

		if (IsInit())
		{
			characterData.LoadCharacterData();
			missiondata.LoadMissionData();
		}
		else
		{
			Init();
		}
	}

	private void OnApplicationQuit()
	{
		SaveCharacterData();
		SaveMissiondata();

		isInit = true;
		DataUtility.Save("IsFirst", isInit);
	}
	public void Init()
	{

		characterData.InitCharacterData();
		missiondata.InitMissionData();

		SaveCharacterData();
		SaveMissiondata();

	}
	public bool IsInit()
	{
		return DataUtility.Load("IsFirst", false);
	}

	public void SaveCharacterData()
	{
		DataUtility.Save("CharacterData", characterData);
	}
	public void SaveMissiondata()
	{
		DataUtility.Save("MissionData", missiondata);
	}

	#region CharacterData
	public class CharacterData
	{
		private float moveSpeed;
		public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

		private float attackPerSecond;
		public float AttackPerSecond { get => attackPerSecond; set => attackPerSecond = value; }

		private BigInteger weaponDamage;
		public BigInteger WeaponDamage { get => weaponDamage; set => weaponDamage = value; }

		private BigInteger criticalDamage;
		public BigInteger CriticalDamage { get => criticalDamage; set => criticalDamage = value; }

		private BigInteger criticalChance;
		public BigInteger CriticalChance { get => criticalChance; set => criticalChance = value; }

		private BigInteger treasureDamagePer;
		public BigInteger TreasureDamagePer { get => treasureDamagePer; set => treasureDamagePer = value; }

		private BigInteger treasureCriticalDamagePer;
		public BigInteger TreasureCriticalDamagePer { get => treasureCriticalDamagePer; set => treasureCriticalDamagePer = value; }

		private BigInteger treasureEnemyGoldPer;
		public BigInteger TreasureEnemyGoldPer { get => treasureEnemyGoldPer; set => treasureEnemyGoldPer = value; }

		private BigInteger treasureQuestGoldPer;
		public BigInteger TreasureQuestGoldPer { get => treasureQuestGoldPer; set => treasureQuestGoldPer = value; }

		private BigInteger treasureExtraDamage;
		public BigInteger TreasureExtraDamage { get => treasureExtraDamage; set => treasureExtraDamage = value; }

		public void InitCharacterData()
		{
			MoveSpeed = 1f;
			AttackPerSecond = 1f;
			WeaponDamage = 2;
			CriticalDamage = 2;
			CriticalChance = 100;
			TreasureDamagePer = 0;
			TreasureCriticalDamagePer = 0;
			TreasureEnemyGoldPer = 0;
			TreasureQuestGoldPer = 0;
			TreasureExtraDamage = 0;
		}

		public void LoadCharacterData()
		{
			CharacterData data = DataUtility.Load<CharacterData>("CharacterData");
			MoveSpeed = data.MoveSpeed;
			AttackPerSecond = data.AttackPerSecond;
			WeaponDamage = data.WeaponDamage;
			CriticalDamage = data.CriticalDamage;
			CriticalChance = data.CriticalChance;
		}
	}
	#endregion

	#region MissionData
	public class MissionData
	{
		private int clearMissionNo;
		public int ClearMissionNo { get => clearMissionNo; set => clearMissionNo = value; }

		private Dictionary<int, int> questUpgradeData = new();

		/// <summary>
		/// questNo, questLevel
		/// </summary>
		public Dictionary<int, int> QuestUpgradeData { get => questUpgradeData; set => questUpgradeData = value; }

		private Dictionary<int, int> questClearData = new();

		/// <summary>
		/// questNo, questClearCount
		/// </summary>
		public Dictionary<int, int> QuestClearData { get => questClearData; set => questClearData = value; }

		private Dictionary<int, int> weaponUpgradeData = new();
		/// <summary>
		/// weaponNo, weaponLevel
		/// </summary>
		public Dictionary<int, int> WeaponUpgradeData { get => weaponUpgradeData; set => weaponUpgradeData = value; }

		private Dictionary<int, int> dungeonClearData = new();

		/// <summary>
		/// stageNo, stageClearCount
		/// </summary>
		public Dictionary<int, int> DungeonClearData { get => dungeonClearData; set => dungeonClearData = value; }

		public void InitMissionData()
		{
			ClearMissionNo = 0;
		}

		public void LoadMissionData()
		{
			MissionData data = DataUtility.Load<MissionData>("MissionData");
			ClearMissionNo = data.ClearMissionNo;
			QuestUpgradeData = data.QuestUpgradeData;
			QuestClearData = data.QuestClearData;
			WeaponUpgradeData = data.WeaponUpgradeData;
			DungeonClearData = data.DungeonClearData;
		}

		public void UpdateQuestUpgradeData(int questNo, Func<int, int> updateFunc)
		{
			QuestUpgradeData.AddOrUpdate(questNo, updateFunc, 1);
		}

		public void UpdateQuestClearData(int questNo, Func<int, int> updateFunc)
		{
			QuestClearData.AddOrUpdate(questNo, updateFunc, 1);
		}

		public void UpdateWeaponUpgradeData(int weaponNo, Func<int, int> updateFunc)
		{
			WeaponUpgradeData.AddOrUpdate(weaponNo, updateFunc, 1);
		}

		public void UpdateDungeonClearData(int dungeonNo, Func<int, int> updateFunc)
		{
			DungeonClearData.AddOrUpdate(dungeonNo, updateFunc, 1);
		}
	}
	#endregion
}
