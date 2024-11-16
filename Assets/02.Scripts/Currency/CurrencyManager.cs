using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlphabetNumber;
using ThreeRabbitPackage.DesignPattern;
using UniRx;
using System;

public static class CurrencyManager<T>
{
    private static readonly Dictionary<ECurrencyType, T> currencyDictionary = new();

    public static void RegisterCurrency(ECurrencyType type, T currency)
	{
        if (!currencyDictionary.ContainsKey(type))
		{
            currencyDictionary[type] = currency;
		}
	}

    public static T GetCurrency(ECurrencyType type)
	{
        if (!currencyDictionary.TryGetValue(type, out var currency))
        {
            throw new ArgumentException($"CurrencyManager: Currency {type} is not registered.");
        }

        return currency;
    }

 //   public bool AddCurrency(ECurrencyType currencyType, BigInteger amount)
	//{
 //       var currency = UserDataManager.Instance.currencyData.GetCurrency(currencyType);
 //       var isPositive = currency.Value.IsPositive(amount);

 //       if (!isPositive) return false;

 //       currency.Value += amount;
 //       return true;
 //   }


 //   public bool IsEnughCurrency(ECurrencyType currencyType, BigInteger amount)
 //   {
 //       var currency = UserDataManager.Instance.currencyData.GetCurrency(currencyType);
 //       var isEnugh = currency.Value.IsPositive(amount);

 //       return isEnugh;
 //   }

 //   public void test()
 //   {
 //       BigInteger gold = 10000;
 //       AddCurrency(ECurrencyType.GOLD , gold);
 //   }
}
