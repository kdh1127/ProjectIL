using System.Numerics;
using System.Collections.Generic;
using ThreeRabbitPackage.DesignPattern;
using UniRx;
using UnityEngine;
using System;

public class UserDataManager : TRSingleton<UserDataManager>
{
	public CurrencyData currencyData = new();
	public CharacterData characterData = new();
	public Missiondata missiondata = new();
	private bool isInit = false;
	private new void Awake()
	{
		base.Awake();

		if (IsInit())
		{
			currencyData.LoadCurrencyData();
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
		SaveCurrencyData();
		SaveCharacterData();
		SaveMissiondata();

		isInit = true;
		DataUtility.Save("IsFirst", isInit);
	}
	public void Init()
	{

		currencyData.InitCurrencyData();
		characterData.InitCharacterData();
		missiondata.InitMissionData();

		SaveCurrencyData();
		SaveCharacterData();
		SaveMissiondata();

	}
	public bool IsInit()
	{
		return DataUtility.Load("IsFirst", false);
	}

	public void SaveCurrencyData()
	{
		DataUtility.Save("CurrencyData", currencyData);
	}
	public void SaveCharacterData()
	{
		DataUtility.Save("CharacterData", characterData);
	}
	public void SaveMissiondata()
	{
		DataUtility.Save("MissionData", missiondata);
	}

	#region CurrencyData
	public class CurrencyData
	{
		private ReactiveProperty<BigInteger> gold = new();
		public ReactiveProperty<BigInteger> Gold { get => gold; set => gold = value; }

		private ReactiveProperty<BigInteger> dia = new();
		public ReactiveProperty<BigInteger> Dia { get => dia; set => dia = value; }

		private ReactiveProperty<BigInteger> key = new();
		public ReactiveProperty<BigInteger> Key { get => key; set => key = value; }

		public ReactiveProperty<BigInteger> GetCurrency(ECurrencyType currencyType)
		{
			switch (currencyType)
			{
				case ECurrencyType.GOLD:
					return Gold;
				case ECurrencyType.DIA:
					return Dia;
				case ECurrencyType.KEY:
					return Key;
			}

			return null;
		}
		public void InitCurrencyData()
		{
			Gold.Value = 0;
			Dia.Value = 0;
			Key.Value = 0;
		}

		public void LoadCurrencyData()
		{
			CurrencyData data = DataUtility.Load<CurrencyData>("CurrencyData");
			Gold.Value = data.Gold.Value;
			Dia.Value = data.Dia.Value;
			Key.Value = data.Key.Value;
		}
	}
	#endregion

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

		public void InitCharacterData()
		{
			MoveSpeed = 1f;
			AttackPerSecond = 1f;
			WeaponDamage = 2;
			CriticalDamage = 2;
			CriticalChance = 10;
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
	public class Missiondata
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
			Missiondata data = DataUtility.Load<Missiondata>("MissionData");
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
