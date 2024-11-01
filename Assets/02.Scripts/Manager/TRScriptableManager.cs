using UnityEngine;
using ThreeRabbitPackage;
using ThreeRabbitPackage.DesignPattern;

public class TRScriptableManager : TRSingleton<TRScriptableManager>
{
	[SerializeField] public TRGoogleSheetDictionary GoogleSheet;
	[SerializeField] public TRGameObjectResourcesDictionary GameObject;
	[SerializeField] public TRSpriteResourcesDictionary Sprite;

	public void Awake()
	{
		base.Awake();

		GoogleSheet = this.GoogleSheet;
	}
}

[System.Serializable]
public class TRGoogleSheetDictionary : SerializableDictionary<string, TRGoogleSheet> { }

[System.Serializable]
public class TRSpriteResourcesDictionary : SerializableDictionary<string, TRSpriteResources> { }

[System.Serializable]
public class TRGameObjectResourcesDictionary : SerializableDictionary<string, TRGameObjectResources> { }