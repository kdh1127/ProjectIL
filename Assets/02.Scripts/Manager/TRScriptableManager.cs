using System.Collections.Generic;
using UnityEngine;
using ThreeRabbitPackage;
using ThreeRabbitPackage.DesignPattern;

public class TRScriptableManager : TRSingleton<TRScriptableManager>
{
	[SerializeField] private List<TRGoogleSheet> googleSheetList;
	[SerializeField] private List<TRGameObjectResources> gameObectList;
	[SerializeField] private List<TRSpriteResources> spriteList;

	public TRGoogleSheet GetGoogleSheet(string name)
	{
		return googleSheetList.Find(obj => obj.name == name);
	}

	public TRGameObjectResources GetGameObject(string name)
	{
		return gameObectList.Find(obj => obj.name == name);
	}

	public TRSpriteResources GetSprite(string name)
	{
		return spriteList.Find(obj => obj.name == name);
	}
}