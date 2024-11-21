using System.Collections.Generic;
using System.Numerics;
using Zenject;
public class CurrencyModel
{
    public class Gold : CurrencyBase { }
    public class Dia : CurrencyBase { }
    public class Key : CurrencyBase { }

    public Gold gold;
    public Dia dia;
    public Key key;

    private Dictionary<ECurrencyType, CurrencyBase> currencies = new();

    [Inject]
    public CurrencyModel(
        Gold gold, 
        Dia dia,
        Key key)
    {
        this.gold = gold;
        this.dia = dia;
        this.key = key;
    }

    public void Init()
	{
        currencies.Add(ECurrencyType.GOLD, gold);
        currencies.Add(ECurrencyType.DIA, dia);
        currencies.Add(ECurrencyType.KEY, key);
	}

    public void AddCurrency(ECurrencyType type, BigInteger amount)
	{
        currencies[type].Add(amount);
    }

    public bool SubCurrency(ECurrencyType type, BigInteger amount)
	{
        if(currencies[type].CanSubtract(amount))
		{
            currencies[type].Subtract(amount);
            return true;
		}

        return false;
	}
}