using System.Collections.Generic;
using UnityEngine;

public interface IToggleView
{
	public void ActivatePanel(CommonToggle.ToggleType toggleType);
	public void DeactivateAllPanel(List<GameObject> panelList);
}
