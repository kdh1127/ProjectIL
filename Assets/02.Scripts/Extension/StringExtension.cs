using System;
using System.Numerics;

public static class StringExtension
{
    public static BigInteger ToBigInt(this string str)
	{
		return BigInteger.Parse(str);
	}

	public static T ToEnum<T>(this string str) where T : struct
    {
        if (Enum.TryParse<T>(str, out T res) && Enum.IsDefined(typeof(T), res))
        {
            return res;
        }
        return default;
    }
}
