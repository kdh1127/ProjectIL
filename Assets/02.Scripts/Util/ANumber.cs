using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Text.RegularExpressions;

namespace AlphabetNumber
{
	internal static class Dictionary
	{
		internal static Dictionary<string, int> alphabetNumberDictionary = new Dictionary<string, int>();
		static List<char> alphabet = new List<char> { 'Z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y' };

		static Dictionary()
		{
			for (int i = 0; i < 1000; i++)
			{
				alphabetNumberDictionary.Add(Gen(i), i);
			}
		}
		static string Gen(int x)
		{
			string s = "";
			int num = x;
			while (num != 0)
			{
				int div = num-- % 26;
				s = alphabet[div] + s;
				num /= 26;
			}
			return s;
		}
	}
	public static class Conversion
	{
		/// <summary>
		/// ���ڸ� ���ڷ� ��ȯ�մϴ�. ��) 1000 => 1A�� ��ȯ
		/// </summary>
		/// <param name="number">��ȯ�� ����</param>
		/// <param name="count">��͸� ���� ����Ʈ �Ű�����</param>
		/// <returns></returns>
		public static string BigIntegerToAlphabetNumber(BigInteger number, int count = 0)
		{
			for (count = 0; count < Dictionary.alphabetNumberDictionary.Count; count++)
			{
				if (number >= 1 && number < 1000)
				{
					return (number.ToString() + Dictionary.alphabetNumberDictionary.Keys.ElementAt(count));
				}
				//"0"�� �ٲ� �� ����ó��
				else if (number == 0)
				{
					return "0";
				}
				else
					number /= 1000;
			}

			return "ERROR";
		}

		/// <summary>
		/// ���ڸ� ���ڷ� ��ȯ�մϴ�. ��) 1A => 1000���� ��ȯ
		/// </summary>
		/// <param name="number">��ȯ�� ����</param>
		/// <returns></returns>
		public static BigInteger AlphabetNumberToBigInteger(string number)
		{
			//���ڿ� ����ó��
			if (number == "0") return 0;
			int value = int.Parse(Regex.Replace(number, @"\\D", ""));
			string key = Regex.Replace(number, @"\\d", "");

			BigInteger powUnit = BigInteger.Pow(1000, Dictionary.alphabetNumberDictionary[key]);

			return value * powUnit;
		}
	}

	public class ANumber
	{
		public BigInteger bigInteger;

		#region Creator
		public ANumber() { }
		public ANumber(BigInteger bigInteger) { this.bigInteger = bigInteger; }
		public ANumber(string stringNumber) { bigInteger = Conversion.AlphabetNumberToBigInteger(stringNumber); }
		#endregion

		#region Add operator
		public static ANumber operator +(ANumber alphabetNumber1, ANumber alphabetNumber2)
		{
			BigInteger returnNumber = BigInteger.Add(alphabetNumber1.bigInteger, alphabetNumber2.bigInteger);
			return new ANumber(returnNumber);
		}
		public static ANumber operator +(ANumber alphabetNumber, BigInteger bigInteger)
		{
			BigInteger returnNumber = BigInteger.Add(alphabetNumber.bigInteger, bigInteger);
			return new ANumber(returnNumber);
		}
		public static ANumber operator +(ANumber alphabetNumber, string stringNumber)
		{
			BigInteger conversion = Conversion.AlphabetNumberToBigInteger(stringNumber);
			BigInteger returnNumber = BigInteger.Add(alphabetNumber.bigInteger, conversion);

			return new ANumber(returnNumber);
		}
		#endregion

		#region Subtrac operator
		public static ANumber operator -(ANumber alphabetNumber1, ANumber alphabetNumber2)
		{
			BigInteger returnNumber = BigInteger.Subtract(alphabetNumber1.bigInteger, alphabetNumber2.bigInteger);
			return new ANumber(returnNumber);
		}
		public static ANumber operator -(ANumber alphabetNumber, BigInteger bigInteger)
		{
			BigInteger returnNumber = BigInteger.Subtract(alphabetNumber.bigInteger, bigInteger);
			return new ANumber(returnNumber);
		}
		public static ANumber operator -(ANumber alphabetNumber, string stringNumber)
		{
			BigInteger conversion = Conversion.AlphabetNumberToBigInteger(stringNumber);
			BigInteger returnNumber = BigInteger.Subtract(alphabetNumber.bigInteger, conversion);

			return new ANumber(returnNumber);
		}
		#endregion

		#region Multiply operator
		public static ANumber operator *(ANumber alphabetNumber1, ANumber alphabetNumber2)
		{
			BigInteger returnNumber = BigInteger.Multiply(alphabetNumber1.bigInteger, alphabetNumber2.bigInteger);
			return new ANumber(returnNumber);
		}
		public static ANumber operator *(ANumber alphabetNumber, BigInteger bigInteger)
		{
			BigInteger returnNumber = BigInteger.Multiply(alphabetNumber.bigInteger, bigInteger);
			return new ANumber(returnNumber);
		}
		public static ANumber operator *(ANumber alphabetNumber, string stringNumber)
		{
			BigInteger conversion = Conversion.AlphabetNumberToBigInteger(stringNumber);
			BigInteger returnNumber = BigInteger.Multiply(alphabetNumber.bigInteger, conversion);

			return new ANumber(returnNumber);
		}
		#endregion

		#region Divide operator
		public static ANumber operator /(ANumber alphabetNumber1, ANumber alphabetNumber2)
		{
			BigInteger returnNumber = BigInteger.Divide(alphabetNumber1.bigInteger, alphabetNumber2.bigInteger);
			return new ANumber(returnNumber);
		}
		public static ANumber operator /(ANumber alphabetNumber, BigInteger bigInteger)
		{
			BigInteger returnNumber = BigInteger.Divide(alphabetNumber.bigInteger, bigInteger);
			return new ANumber(returnNumber);
		}
		public static ANumber operator /(ANumber alphabetNumber, string stringNumber)
		{
			BigInteger conversion = Conversion.AlphabetNumberToBigInteger(stringNumber);
			BigInteger returnNumber = BigInteger.Divide(alphabetNumber.bigInteger, conversion);

			return new ANumber(returnNumber);
		}
		#endregion
	}

	public static class AlphabetNumberExtension
    {
		public static string ToAlphaString(this ANumber alphabetNubmer, int count = 0)
		{
			var temp = alphabetNubmer.bigInteger;
			for (count = 0; count < Dictionary.alphabetNumberDictionary.Count; count++)
			{
				if (temp >= 1 && temp < 1000)
				{
					return (temp.ToString() + Dictionary.alphabetNumberDictionary.Keys.ElementAt(count));
				}
				//"0"�� �ٲ� �� ����ó��
				else if (temp == 0)
				{
					return "0";
				}
				else
					temp /= 1000;
			}

			return "ERROR";
		}
	}
}