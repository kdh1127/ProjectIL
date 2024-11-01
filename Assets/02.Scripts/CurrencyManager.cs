using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlphabetNumber;
using ThreeRabbitPackage.DesignPattern;
using UniRx;

public class CurrencyManager : TRSingleton<CurrencyManager>
{

    public ReactiveProperty<ANumber> gold = new();
    public ReactiveProperty<ANumber> dia = new();
    public ReactiveProperty<ANumber> key = new();

    private new void Awake()
    {
        base.Awake();

        gold.Value = new(0);
        dia.Value = new(0);
        key.Value = new(0);
    }

    public bool AddGold(BigInteger newGold)
    {
        var isPositive = IsPostive(gold.Value.bigInteger, newGold);

        if (isPositive)
        {
            gold.Value += newGold;
            return true;
        }

        else return false;
    }

    public bool AddDia(BigInteger newDia)
    {
        var isPositive = IsPostive(dia.Value.bigInteger, newDia);

        if (isPositive)
        {
            dia.Value += newDia;
            return true;
        }

        else return false;
    }

    public bool Addkey(BigInteger newKey)
    {
        var isPositive = IsPostive(key.Value.bigInteger, newKey);

        if (isPositive)
        {
            key.Value += newKey;
            return true;
        }

        else return false;
    }

    private bool IsPostive(BigInteger curCurrency, BigInteger newCurrency)
	{
        return (curCurrency += newCurrency) >= 0;
    }

    public void Test()
    {
        gold.Value += 100;
    }
}
