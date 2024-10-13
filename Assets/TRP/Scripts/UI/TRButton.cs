using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ThreeRabbitPackage;

public enum EButtonType
{
	OpenPopup,
	CreatePopup,
	ClickEvent,
	Close,
	None
}

namespace ThreeRabbitPackage
{
	public class TRButton : Button
	{
		public TRSpriteResources UIResources;
		[HideInInspector] public string buttonResource;
		[HideInInspector] public EButtonType buttonType;
		[HideInInspector] public string popupName;
		[HideInInspector] public GameObject closePopup;
		[HideInInspector] public GameObject parent;
		[HideInInspector] public GameObject createPopup;
		[HideInInspector] public int choice;
		public List<string> ButtonList
		{
			get
			{
				List<string> temp = new List<string>();

				if (UIResources != null)
				{
					foreach (string key in UIResources.spriteDictionary.Keys)
					{
						temp.Add(key);
					}
				}
				else
				{
					temp.Add("TRPUIResources가 필요합니다.");
				}

				return temp;
			}

		}

		public event Action OnClick;

		public void Start()
		{
			if(buttonType != EButtonType.None)
				GetComponent<Button>().onClick.AddListener(ClickEvent);
		}

		public void OnDestroy()
		{
			ClearClickEvent();
		}

		public void ClickEvent()
		{
			switch (buttonType)
			{
				case EButtonType.OpenPopup:
					//PopupManager.Instance.OnPopup(popupName);
					break;
				case EButtonType.CreatePopup:
					if (parent != null) Instantiate(createPopup, parent.transform);
					else Instantiate(createPopup);
					break;
				case EButtonType.ClickEvent:
					OnClick?.Invoke();
					break;
				case EButtonType.Close:
					Destroy(closePopup);
					break;

				case EButtonType.None:
					break;
			}
		}

		public void ClearClickEvent()
		{
			if (OnClick == null)
			{
				return;
			}

			foreach (var d in OnClick.GetInvocationList())
			{
				OnClick -= (Action)d;
			}
		}
	}
}