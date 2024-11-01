using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class MainButtonModel
{
	public Subject<CommonToggle> toggleSubject = new();

	public void RegisterToggleList(ToggleGroup toggleGroup, List<CommonToggle> toggleList)
	{
		foreach (var toggle in toggleList)
		{
			toggleGroup.RegisterToggle(toggle);
			toggle.onValueChanged.AddListener(isOn =>
			{
				if(toggle.isOn)
				{
					toggleGroup.NotifyToggleOn(toggle);
					toggleSubject.OnNext(toggle);
				}
			});
		}
	}
}
