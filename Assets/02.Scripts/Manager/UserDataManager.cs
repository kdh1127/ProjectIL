using System.Numerics;
using System.Collections.Generic;
using ThreeRabbitPackage.DesignPattern;
using UniRx;
using System;
using System.Linq;

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
			treasureData.Load();
			skinData.Load();
			stageData.Load();
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
		treasureData.Init();
		skinData.Init();
		stageData.Init();

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
		treasureData.Save();
		skinData.Save();
		stageData.Save();
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
			CriticalChance = 10;
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
			TreasureDamagePer = data.TreasureDamagePer;
			TreasureCriticalDamagePer = data.TreasureCriticalDamagePer;
			TreasureEnemyGoldPer = data.TreasureEnemyGoldPer;
			TreasureQuestGoldPer = data.treasureQuestGoldPer;
			TreasureExtraDamage = data.TreasureExtraDamage;
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

		private Dictionary<int, int> buySkinData = new();

		/// <summary>
		/// stageNo, stageClearCount
		/// </summary>
		public Dictionary<int, int> BuySkinData { get => buySkinData; set => buySkinData = value; }

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
			BuySkinData = data.BuySkinData;
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

		public void UpdateBuySkinData(int skinNo, Func<int, int> updateFunc)
		{
			BuySkinData.AddOrUpdate(skinNo, updateFunc, 1);
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

	#region TreasureData
	public TreasureData treasureData = new();

	public class TreasureData
	{
		public class Treasure
		{
			public int level = 0;
		}
		public List<Treasure> treasureList = new();

		public void Init()
		{
			TreasureTableList.Get().ForEach(table =>
			{
				treasureList.Add(new Treasure());
			});
		}

		public void Load()
		{
			TreasureData data = DataUtility.Load<TreasureData>("TreasureData");
			treasureList = data.treasureList;
		}

		public void Save()
		{
			DataUtility.Save("TreasureData", Instance.treasureData);
		}
	}
	#endregion

	#region SkinData
	public SkinData skinData = new();

	public class SkinData
	{
		public class Skin
		{
			public int level = 0;
			public bool isEquip = false;
		}
		public List<Skin> skinList = new();
		public int originWeaponNo;
		public int equipSkinNo;

		public void Init()
		{
			SkinTableList.Get().ForEach(table =>
			{
				skinList.Add(new Skin());
			});
			originWeaponNo = 0;
			equipSkinNo = 0;
		}

		public void Load()
		{
			SkinData data = DataUtility.Load<SkinData>("SkinData");
			skinList = data.skinList;
			originWeaponNo = data.originWeaponNo;
			equipSkinNo = data.equipSkinNo;
		}

		public void Save()
		{
			DataUtility.Save("SkinData", Instance.skinData);
		}
	}
	#endregion

	#region StageData
	public StageData stageData = new();

	public class StageData
	{
		public class Stage
		{
			public int curStage = 0;
			public BigInteger stageBaseHp = 500;
		}

		public Stage stage = new();

		public void Init()
		{
			stage.curStage = 0;
			stage.stageBaseHp = 500;
		}

		public void Load()
		{
			StageData data = DataUtility.Load<StageData>("StageData");
			stage = data.stage;
		}

		public void Save()
		{
			DataUtility.Save("StageData", Instance.stageData);
		}
	}
	#endregion
}
