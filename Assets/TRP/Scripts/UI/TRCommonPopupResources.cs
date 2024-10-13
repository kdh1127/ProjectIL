using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThreeRabbitPackage;


[CreateAssetMenu(fileName = "CommonPopupResources", menuName = "Scriptable Object/CommonPopup", order = int.MaxValue)]
public class TRCommonPopupResources : ScriptableObject
{
    [Header("CommonPopup Background Settings")]
    public TRSpriteDictionary backgroundResources;

    [Header("CommonPopup Buttons Settings")]
    public GameObject close_btn;
    public GameObject ok_btn;
    public GameObject cancel_btn;
}
