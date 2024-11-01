using System;
using System.Numerics;

public static class BigIntegerExtension
{
    public static string ToAlphabetNumber(this BigInteger value)
	{
        BigInteger baseValue = 1000;
        int exponent = 0;

        while (value >= BigInteger.Pow(baseValue, exponent + 1))
        {
            exponent++;
        }

        decimal numberPart = (decimal)value / (decimal)BigInteger.Pow(baseValue, exponent);
        string formattedNumber = Math.Round(numberPart, 1).ToString();

        string alphabetPart = GetAlphabetSuffix(exponent);

        return formattedNumber + alphabetPart;
    }

    private static string GetAlphabetSuffix(int exponent)
    {
        string result = string.Empty;
        while (exponent >= 0)
        {
            result = (char)('A' + (exponent % 26)) + result;
            exponent = exponent / 26 - 1;
        }
        return result;
    }
}
