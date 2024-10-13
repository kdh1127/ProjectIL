using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThreeRabbitPackage;
using UnityEngine.UI;

public class TRPopupMaker : TRPopup
{
	[Header("TRCommonPopup Settings")] public TRCommonPopupResources commonPopupResources;
	[HideInInspector] public string bgResource;
	[HideInInspector] public int choice;
	public List<string> bgList
	{
		get
		{
			List<string> temp = new List<string>();

			if (commonPopupResources != null)
			{
				foreach (string key in commonPopupResources.backgroundResources.Keys)
				{
					temp.Add(key);
				}
			}
			else
			{
				temp.Add("commonPopupResources 필요합니다.");
			}

			return temp;
		}
	}
}
