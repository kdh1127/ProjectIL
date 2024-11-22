using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainButtonView : MonoBehaviour
{
	public ToggleGroup toggleGroup;
	public List<CommonToggle> toggleList;
	public List<GameObject> panelList;
	public Color activeToggleColor;
	public Color deactiveToggleColor;
	private void Awake()
	{
		RegisterToggleList(toggleGroup, toggleList);
	}
	public void RegisterToggleList(ToggleGroup toggleGroup, List<CommonToggle> toggleList)
	{
		foreach (var toggle in toggleList)
		{
			toggleGroup.RegisterToggle(toggle);
			toggle.onValueChanged.AddListener(isOn =>
			{
				if (toggle.isOn)
				{
					toggleGroup.NotifyToggleOn(toggle);
					DeactivateAllPanel();
					ActivatePanel(toggle.type);
					ChangeToggleColor(toggle, activeToggleColor);
					toggle.interactable = false;
				}
				else
				{
					ChangeToggleColor(toggle, deactiveToggleColor);
					toggle.interactable = true;
				}
			});
		}
	}

	public void ActivatePanel(CommonToggle.ToggleType toggleType)
	{
		switch (toggleType)
		{
			case CommonToggle.ToggleType.Quest:
				panelList[0].SetActive(true);
				break;
			case CommonToggle.ToggleType.Weapon:
				panelList[1].SetActive(true);
				break;
			case CommonToggle.ToggleType.Mission:
				panelList[2].SetActive(true);
				break;
			case CommonToggle.ToggleType.Treasure:
				panelList[3].SetActive(true);
				break;
		}
	}

	public void DeactivateAllPanel()
	{
		panelList.ForEach(panel =>
		{
			panel.SetActive(false);
		});
	}

	public void ChangeToggleColor(CommonToggle toggle, Color color)
	{
		var toggleImage = toggle.GetComponentInChildren<Image>(); 
		if (toggleImage != null)
		{
			toggleImage.color = color;
		}
	}
}
