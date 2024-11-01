using System.Numerics;
using System.Collections.Generic;
using ThreeRabbitPackage.DesignPattern;
using UniRx;

public class UserDataManager : TRSingleton<UserDataManager>
{
	public CurrencyData currencyData = new();
	public CharacterData characterData = new();
	private new void Awake()
	{
		base.Awake();
		Init();
	}
	public void Init()
	{
		currencyData.InitCurrencyData();
		characterData.InitCharacterData();
	}

	#region CurrencyData
	public class CurrencyData
	{
		private Dictionary<EnumList.ECurrencyType, ReactiveProperty<BigInteger>> currency = new();
		public Dictionary<EnumList.ECurrencyType, ReactiveProperty<BigInteger>> Currency { get => currency; set => currency = value; }

		public void InitCurrencyData()
		{
			Currency.Add(EnumList.ECurrencyType.GOLD, new ReactiveProperty<BigInteger>());
			Currency.Add(EnumList.ECurrencyType.DIA, new ReactiveProperty<BigInteger>());
			Currency.Add(EnumList.ECurrencyType.KEY, new ReactiveProperty<BigInteger>());
		}

		public void SaveCurrencyData()
		{

		}

		public void LoadCurrencyData()
		{

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
			WeaponDamage = 0;
			CriticalDamage = 2;
			CriticalChance = 10;
		}

		public void SaveCharacterData()
		{

		}

		public void LoadCharacterData()
		{

		}
	}
	#endregion
}
