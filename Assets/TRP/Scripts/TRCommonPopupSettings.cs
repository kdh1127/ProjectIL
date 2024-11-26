using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TRCommonPopupSettings", menuName = "ThreeRabbitPackage/CommonPopup", order = int.MaxValue)]
public class TRCommonPopupSettings : ScriptableObject
{
	public GameObject TRCommonPoupupPrefab;
	public Transform parent;
}
