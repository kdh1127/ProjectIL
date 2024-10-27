using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThreeRabbitPackage;
using ThreeRabbitPackage.DesignPattern;

public class TRScriptableManager : TRSingleton<TRScriptableManager>
{
    public TRGoogleSheetDictionary GoogleSheet;
    public TRSpriteResourcesDictionary Sprite;
    

}

[System.Serializable]
public class TRSpriteResourcesDictionary : SerializableDictionary<string, TRSpriteResources> { }

[System.Serializable]
public class TRGoogleSheetDictionary : SerializableDictionary<string, TRGoogleSheet> { }