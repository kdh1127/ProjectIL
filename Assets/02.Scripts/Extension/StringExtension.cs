using System.Numerics;

public static class StringExtension
{
    public static BigInteger ToBigInteger(this string str)
	{
		return BigInteger.Parse(str);
	}
}
