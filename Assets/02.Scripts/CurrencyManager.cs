using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlphabetNumber;
using ThreeRabbitPackage.DesignPattern;
using UniRx;

public class CurrencyManager : TRSingleton<CurrencyManager>
{
    private new void Awake()
    {
        base.Awake();
    }

    public bool AddCurrency(EnumList.ECurrencyType currencyType, BigInteger amount)
	{
        var currency = UserDataManager.Instance.currencyData.Currency[currencyType];
        var isPositive = IsPositiveAmount(currency.Value, amount);

        if (!isPositive) return false;

        currency.Value += amount;
        return true;
    }

    public bool IsPositiveAmount(BigInteger curCurrency, BigInteger newCurrency)
	{
        return (curCurrency += newCurrency) >= 0;
    }

    public bool IsEnughCurrency(EnumList.ECurrencyType currencyType, BigInteger amount)
    {
        var currency = UserDataManager.Instance.currencyData.Currency[currencyType];
        var isEnugh = IsPositiveAmount(currency.Value, amount);

        return isEnugh;
    }

    public void test()
    {
        BigInteger gold = 10000;
        AddCurrency(EnumList.ECurrencyType.GOLD , gold);
    }
}
