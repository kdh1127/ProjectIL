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

     void Awake()
    {
        gold.Value = new(0);
        dia.Value = new(0);
        key.Value = new(0);
    }

    public bool AddGold(BigInteger newGold)
    {
        var subValue = gold.Value - newGold;

        if (subValue.bigInteger >= 0)
        {
            gold.Value += newGold;
            return true;
        }

        else return false;
    }

    public void Test()
    {
        gold.Value += 100;
        dia.Value += 1000;
    }
}
