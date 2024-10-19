using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainButtonView : MonoBehaviour, IToggleView
{
	public List<CommonToggle> toggleList;

	public void ActivatePanel(CommonToggle.ToggleType toggleType)
	{
		switch (toggleType)
		{
			case CommonToggle.ToggleType.Quest:
				break;
			case CommonToggle.ToggleType.Weapon:
				break;
			case CommonToggle.ToggleType.Shop:
				break;
		}
	}

	public void DeactivateAllPanel()
	{
	}
}
