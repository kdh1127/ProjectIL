public enum ECurrencyType
{
	GOLD,
	DIA,
	KEY
}

public enum EMonsterType
{
	NORMAL,
	BOSS,
	TEN_BOSS,
	HUNDRED_BOSS
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

public enum EIncreaseType
{
	EnemyGold,
	QuestGold,
	CriticalDamage,
	Damage,
	ExtraDamage
}

public enum EBattleState
{
	Init,
	Lull,
	Clear,
	Battle,
	Reset
}
