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

    public bool AddCurrency(ECurrencyType currencyType, BigInteger amount)
	{
        var currency = UserDataManager.Instance.currencyData.GetCurrency(currencyType);
        var isPositive = IsPositiveAmount(currency.Value, amount);

        if (!isPositive) return false;

        currency.Value += amount;
        return true;
    }

    public bool IsPositiveAmount(BigInteger curCurrency, BigInteger newCurrency)
	{
        return (curCurrency += newCurrency) >= 0;
    }

    public bool IsEnughCurrency(ECurrencyType currencyType, BigInteger amount)
    {
        var currency = UserDataManager.Instance.currencyData.GetCurrency(currencyType);
        var isEnugh = IsPositiveAmount(currency.Value, amount);

        return isEnugh;
    }

    public void test()
    {
        BigInteger gold = 10000;
        AddCurrency(ECurrencyType.GOLD , gold);
    }
}
