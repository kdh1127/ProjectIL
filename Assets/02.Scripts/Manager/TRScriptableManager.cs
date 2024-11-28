using System.Collections.Generic;
using UnityEngine;
using ThreeRabbitPackage;
using ThreeRabbitPackage.DesignPattern;

public class TRScriptableManager : TRSingleton<TRScriptableManager>
{
	[SerializeField] private List<TRGoogleSheet> googleSheetList;
	[SerializeField] private List<TRGameObjectResources> gameObectList;
	[SerializeField] private List<TRSpriteResources> spriteList;
	[SerializeField] private List<TRColorResources> colorList;

	public TRGoogleSheet GetGoogleSheet(string name)
	{
		return googleSheetList.Find(resource => resource.name == name);
	}

	public TRGameObjectResources GetGameObject(string name)
	{
		return gameObectList.Find(resource => resource.name == name);
	}

	public TRSpriteResources GetSprite(string name)
	{
		return spriteList.Find(resource => resource.name == name);
	}

	public TRColorResources GetColor(string name)
	{
		return colorList.Find(resource => resource.name == name);
	}
}