using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainButtonView : MonoBehaviour, IToggleView
{
	public ToggleGroup toggleGroup;
	public List<CommonToggle> toggleList;

	// TODO: panelList의 상수 인덱스 접근이 어떤 패널인지 명확하지 않다.
	// TODO: MainButtonView가 BottomPanelView를 알아야 하는 것이 과연 맞는가?
	public void ActivatePanel(CommonToggle.ToggleType toggleType)
	{
		var bottomPanelView = MainScenePresenter.Instance.bottomPanelView;

		switch (toggleType)
		{
			case CommonToggle.ToggleType.Quest:
				bottomPanelView.panelList[0].SetActive(true);
				break;
			case CommonToggle.ToggleType.Weapon:
				Debug.Log("웨폰 활성화");
				break;
			case CommonToggle.ToggleType.Shop:
				Debug.Log("Shop 활성화");
				break;
		}
	}

	public void DeactivateAllPanel(List<GameObject> panelList)
	{
		panelList.ForEach(panel =>
		{
			panel.SetActive(false);
		});
	}
}
