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
}
