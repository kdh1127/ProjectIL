using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumList
{
	public enum ECurrencyType
	{
		GOLD,
		DIA,
		KEY
	}

	public enum EMonsterType
	{
		NORMAL,
		BOSS
	}

	public enum EWeaponItemUpgradeStatus
	{
		MaxLevel,
		Upgradeable,
		NotUpgradeable,
		CurrentWeapon,
		MaxLevelCurrentWeapon
	}

	public enum EMissionType
    {
		QuestUpgrade,
		QuestClear,
		WeaponUpgrade,
		DungeonClear
	}

	public static T StringToEnum<T>(string str) where T : struct
	{
		try
		{
			T res = (T)Enum.Parse(typeof(T), str);
			if (!Enum.IsDefined(typeof(T), res)) return default(T);
			return res;
		}
		catch
		{
			return default(T);
		}
	}
}
