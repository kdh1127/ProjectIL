using System.Numerics;
using System.Collections.Generic;
using ThreeRabbitPackage.DesignPattern;
using UniRx;
using UnityEngine;
using System;
using System.Linq;
using Zenject;
using Newtonsoft.Json;

public class UserDataManager : TRSingleton<UserDataManager>
{
	private bool isInit = false;
	private new void Awake()
	{
		base.Awake();

		if (IsInit())
		{
			characterData.Load();
			missionData.Load();
			questData.Load();
			currencyData.Load();
			weaponData.Load();
		}
		else
		{
			Init();
		}
	}

	private void OnApplicationQuit()
	{
		SaveAll();

		isInit = true;
		DataUtility.Save("IsFirst", isInit);
	}
	public void Init()
	{
		questData.Init();
		characterData.Init();
		missionData.Init();
		currencyData.Init();
		weaponData.Init();

		SaveAll();
	}
	public bool IsInit()
	{
		return DataUtility.Load("IsFirst", false);
	}

	public void SaveAll()
	{
		characterData.Save();
		missionData.Save();
		questData.Save();
		currencyData.Save();
		weaponData.Save();
	}

	#region CharacterData
	public CharacterData characterData = new();

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

		public Dictionary<ESkinIncreaseType, BigInteger> SkinStatDictionary = new();

		public void Init()
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

			Enum.GetValues(typeof(ESkinIncreaseType))
				.Cast<int>()
				.ToList()
				.ForEach(increaseType => SkinStatDictionary.Add((ESkinIncreaseType)increaseType, 0));
		}

		public void Load()
		{
			CharacterData data = DataUtility.Load<CharacterData>("CharacterData");
			MoveSpeed = data.MoveSpeed;
			AttackPerSecond = data.AttackPerSecond;
			WeaponDamage = data.WeaponDamage;
			CriticalDamage = data.CriticalDamage;
			CriticalChance = data.CriticalChance;
			SkinStatDictionary = data.SkinStatDictionary;
		}

		public void Save()
		{
			DataUtility.Save("CharacterData", UserDataManager.Instance.characterData);
		}
	}
	#endregion

	#region MissionData
	public MissionData missionData = new();

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

		public void Init()
		{
			ClearMissionNo = 0;
		}

		public void Load()
		{
			MissionData data = DataUtility.Load<MissionData>("MissionData");
			ClearMissionNo = data.ClearMissionNo;
			QuestUpgradeData = data.QuestUpgradeData;
			QuestClearData = data.QuestClearData;
			WeaponUpgradeData = data.WeaponUpgradeData;
			DungeonClearData = data.DungeonClearData;
		}

		public void Save()
		{
			DataUtility.Save("MissionData", UserDataManager.Instance.missionData);
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

	#region QuestData
	public QuestData questData = new();

	public class QuestData
	{
		public class Quest
		{
			public int level;
			public int elpasedTime;
		}
		public Dictionary<int, Quest> questDict = new();

		public void Init()
		{
			QuestTableList.Get().ForEach(questTable =>
			{
				questDict[questTable.QuestNo] = new Quest();
			});
		}

		public void Load()
		{
			QuestData data = DataUtility.Load<QuestData>("QuestData");
			questDict = data.questDict;
		}

		public void Save()
		{
			DataUtility.Save("QuestData", UserDataManager.Instance.questData);
		}
	}
	#endregion

	#region CurrencyData
	public CurrencyData currencyData = new();

	public class CurrencyData
	{
		public BigInteger gold;
		public BigInteger dia;
		public BigInteger key;

		public void Init()
		{
			gold = 0;
			dia = 0;
			key = 0;
		}

		public void Load()
		{
			CurrencyData data = DataUtility.Load<CurrencyData>("CurrencyData");
			gold = data.gold;
			dia = data.dia;
			key = data.key;
		}

		public void Save()
		{
			DataUtility.Save("CurrencyData", Instance.currencyData);
		}
	}
	#endregion


	#region WeaponData
	public WeaponData weaponData = new();

	public class WeaponData
	{
		public class WeaponItemData
		{
			public int level;
			public bool isEquip;
			public bool isMaxLevel;
			public bool isUnlock;
		}

		public List<WeaponItemData> weaponItemList = new();

		public void Init()
		{
			WeaponTableList.Get().ForEach(weaponTable =>
			{
				weaponItemList.Add(new WeaponItemData());
			});
		}

		public void Load()
		{
			WeaponData data = DataUtility.Load<WeaponData>("WeaponData");
			weaponItemList = data.weaponItemList;
		}

		public void Save()
		{
			DataUtility.Save("WeaponData", Instance.weaponData);
		}
	}
	#endregion

}
