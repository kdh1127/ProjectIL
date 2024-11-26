using System;
using System.Numerics;
using UnityEngine;

public static class BigIntegerExtension
{
    public static int ToInt(this BigInteger value)
    {
        if (value <= int.MaxValue && value >= int.MinValue)
        {
            return (int)value;
        }
        else
        {
            Debug.LogError("Not Convert BigInteger to int value");
            return -1;
        }
    }
    public static string ToAlphabetNumber(this BigInteger value)
    {
        BigInteger baseValue = 1000;

        if (value < baseValue)
        {
            return value.ToString();
        }

        int exponent = 0;

        while (value >= BigInteger.Pow(baseValue, exponent + 1))
        {
            exponent++;
        }

        BigInteger divisor = BigInteger.Pow(baseValue, exponent);
        BigInteger wholePart = value / divisor;
        BigInteger remainder = value % divisor;

        string formattedNumber = wholePart.ToString();
        if (remainder > 0)
        {
            double decimalPart = (double)remainder / (double)divisor;
            formattedNumber += Math.Round(decimalPart, 1).ToString().Substring(1);
        }

        string alphabetPart = GetAlphabetSuffix(exponent);

        return formattedNumber + alphabetPart;
    }

    public static bool IsPositive(this BigInteger value, BigInteger delta)
	{
        return (value += delta) >= 0;
    }

    private static string GetAlphabetSuffix(int exponent)
    {
        string result = string.Empty;

        while (exponent > 0)
        {
            result = (char)('A' + (exponent - 1) % 26) + result;
            exponent = (exponent - 1) / 26;
        }

        return result;
    }
}