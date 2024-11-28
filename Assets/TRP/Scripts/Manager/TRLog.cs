using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TRLog
{
	public static void Green(string message)
	{
		Debug.Log($"<color=#00FF22>Three Rabbit Log</color>\n{message}");
	}

	public static void Yelow(string message)
	{
		Debug.LogWarning($"<color=#ffff00>Three Rabbit Warning</color>\n{message}");

	}

	public static void Red(string message)
	{
		Debug.LogError($"<color=#FF0000>Three Rabbit Error</color>\n{message}");
	}
}
