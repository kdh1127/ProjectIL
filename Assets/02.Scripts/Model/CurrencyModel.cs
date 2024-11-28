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
    private UserDataManager.CurrencyData CurrencyData => UserDataManager.Instance.currencyData;
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
        CurrencyData.Load();

        currencies.Add(ECurrencyType.GOLD, gold);
        currencies.Add(ECurrencyType.DIA, dia);
        currencies.Add(ECurrencyType.KEY, key);

        AddCurrency(ECurrencyType.GOLD, CurrencyData.gold);
        AddCurrency(ECurrencyType.DIA, CurrencyData.dia);
        AddCurrency(ECurrencyType.KEY, CurrencyData.key);
	}

    public void AddCurrency(ECurrencyType type, BigInteger amount)
	{
        currencies[type].Add(amount);
        Save(type);
    }

	public bool SubCurrency(ECurrencyType type, BigInteger amount)
	{
        if(currencies[type].CanSubtract(amount))
		{
            currencies[type].Subtract(amount);
            Save(type);
            return true;
		}

        return false;
	}

    public void Save(ECurrencyType type)
	{
        switch (type)
        {
            case ECurrencyType.GOLD:
                CurrencyData.gold = currencies[type].Amount;
                break;
            case ECurrencyType.DIA:
                CurrencyData.dia = currencies[type].Amount;
                break;
            case ECurrencyType.KEY:
                CurrencyData.key = currencies[type].Amount;
                break;
        }
    }
}