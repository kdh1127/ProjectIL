using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DictionaryExtension
{
    public static void AddOrUpdate<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        TKey key,
        Func<TValue, TValue> updateFunc,
        TValue defaultValue)
    {
        if (dictionary.TryGetValue(key, out TValue existingValue))
        {
            dictionary[key] = updateFunc(existingValue);
        }
        else
        {
            dictionary[key] = defaultValue;
        }
    }
}
