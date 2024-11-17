using System.Collections.Generic;
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
}