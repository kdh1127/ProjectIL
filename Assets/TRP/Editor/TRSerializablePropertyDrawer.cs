using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TRGoogleSheetDictionary))]
[CustomPropertyDrawer(typeof(TRSpriteResourcesDictionary))]
[CustomPropertyDrawer(typeof(TRGameObjectResourcesDictionary))]
public class TRSerializablePropertyDrawer : SerializableDictionaryPropertyDrawer { }