using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using ThreeRabbitPackage;

[CustomEditor(typeof(TRButton))]
public class TRButtonEditor : Editor
{
	TRButton trpButton;

	[MenuItem("GameObject/UI/TRButton")]
	static void Create(MenuCommand menuCommand)
	{
		GameObject trButton = new GameObject("TRButton", typeof(RectTransform), typeof(CanvasRenderer), typeof(TRButton), typeof(Image), typeof(Button));
		GameObjectUtility.SetParentAndAlign(trButton, menuCommand.context as GameObject);
		Undo.RegisterCreatedObjectUndo(trButton, "Create " + trButton.name);
		Selection.activeObject = trButton;
	}

	public void OnEnable()
	{
		trpButton = target as TRButton;
	}
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		EditorGUI.BeginChangeCheck();
		serializedObject.Update();

		// 이미지 세팅
		if (trpButton.ButtonList.Count > 0)
		{
			trpButton.choice = EditorGUILayout.Popup("Image", trpButton.choice, trpButton.ButtonList.ToArray());
			trpButton.buttonResource = trpButton.ButtonList[trpButton.choice];
			SetImage();
		}

		// 버튼 타입 세팅
		trpButton.buttonType = (EButtonType)EditorGUILayout.EnumPopup("ButtonType", trpButton.buttonType, GUILayout.ExpandWidth(true));
		
		// 버튼 타입입에 따른 변수 노출
		switch (trpButton.buttonType)
		{
			case EButtonType.OpenPopup:
				trpButton.popupName = EditorGUILayout.TextField("Popup Name", trpButton.popupName, GUILayout.ExpandWidth(true));
				break;

			case EButtonType.CreatePopup:
				trpButton.parent = (GameObject)EditorGUILayout.ObjectField("Perent", trpButton.parent, typeof(GameObject), true);
				trpButton.createPopup = (GameObject)EditorGUILayout.ObjectField("Create Popup", trpButton.createPopup, typeof(GameObject), true);

				break;
			case EButtonType.ClickEvent:
					break;
			case EButtonType.Close:
				trpButton.closePopup = (GameObject)EditorGUILayout.ObjectField("Close Popup", trpButton.closePopup, typeof(GameObject), true);
				break;
		}

		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
			EditorUtility.SetDirty(trpButton);
		}
	}
	public void SetImage()
	{
		if (trpButton.TryGetComponent(out Image image))
		{
			image.sprite = trpButton.UIResources?.spriteDictionary[trpButton.buttonResource];
		}
	}
}
