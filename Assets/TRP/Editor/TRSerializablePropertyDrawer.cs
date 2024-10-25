using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TRGoogleSheetDictionary))]
[CustomPropertyDrawer(typeof(TRSpriteResourcesDictionary))]
public class TRSerializablePropertyDrawer : SerializableDictionaryPropertyDrawer { }