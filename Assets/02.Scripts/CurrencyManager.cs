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
        var currency = UserDataManager.Instance.Currency[currencyType];
        var isPositive = IsPositiveAmount(currency.Value, amount);

        if (!isPositive) return false;

        currency.Value += amount;
        return true;
    }

    private bool IsPositiveAmount(BigInteger curCurrency, BigInteger newCurrency)
	{
        return (curCurrency += newCurrency) >= 0;
    }
}
