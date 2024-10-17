using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class MainButtonModel : MonoBehaviour
{
	public Subject<Toggle> toggleSubject = new();

	public void ToggleListSubject(List<Toggle> toggleList)
	{
		foreach (var toggle in toggleList)
		{
			toggle.onValueChanged.AddListener(isOn =>
			{
				if(toggle.isOn)
				{

					toggleSubject.OnNext(toggle);
				}
			});
		}
	}
}
