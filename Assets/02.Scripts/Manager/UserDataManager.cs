using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThreeRabbitPackage.DesignPattern;
using UniRx;

public class UserDataManager : TRSingleton<UserDataManager>
{
	private new void Awake()
	{
		base.Awake();
		Init();
	}

	private void Init()
	{
		InitCurrency();
	}

	#region Currency
	private Dictionary<EnumList.ECurrencyType, ReactiveProperty<BigInteger>> currency = new();
	public Dictionary<EnumList.ECurrencyType, ReactiveProperty<BigInteger>> Currency { get => currency; set => currency = value; }
	private void InitCurrency()
	{
		Currency.Add(EnumList.ECurrencyType.GOLD, new ReactiveProperty<BigInteger>());
		Currency.Add(EnumList.ECurrencyType.DIA, new ReactiveProperty<BigInteger>());
		Currency.Add(EnumList.ECurrencyType.KEY, new ReactiveProperty<BigInteger>());
	}
	public void SaveCurrency()
	{

	}
	public void LoadCurrency()
	{

	}
	#endregion
}
