using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(TRPopupMaker))]
public class TRCommonPopupEditor : Editor
{
    private TRPopupMaker trCommonPopup;

    [MenuItem("GameObject/UI/TRCommonPopup")]
    static void Create(MenuCommand menuCommand)
    {
        GameObject trPopup = new GameObject("TRCommonPopup", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(TRPopupMaker));
        GameObjectUtility.SetParentAndAlign(trPopup, menuCommand.context as GameObject);
        trPopup.GetComponent<RectTransform>().localPosition = Vector3.zero;
        trPopup.GetComponent<RectTransform>().localScale = Vector3.one;
        Undo.RegisterCreatedObjectUndo(trPopup, "Create " + trPopup.name);
        Selection.activeObject = trPopup;
    }

    public void OnEnable()
    {
        trCommonPopup = target as TRPopupMaker;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        serializedObject.Update();

        if (trCommonPopup.bgList.Count > 0)
        {
            trCommonPopup.choice = EditorGUILayout.Popup("Image", trCommonPopup.choice, trCommonPopup.bgList.ToArray());
            trCommonPopup.bgResource = trCommonPopup.bgList[trCommonPopup.choice];
            SetImage();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Button Settings", EditorStyles.boldLabel);

        if (GUILayout.Button("Create Close Button"))
        {
            CreateButton(trCommonPopup.commonPopupResources.close_btn, trCommonPopup.transform);
        }

        if (GUILayout.Button("Create Ok Button"))
        {
            CreateButton(trCommonPopup.commonPopupResources.ok_btn, trCommonPopup.transform);
        }

        if (GUILayout.Button("Create Cancel Button"))
        {
            CreateButton(trCommonPopup.commonPopupResources.cancel_btn, trCommonPopup.transform);
        }

        if(GUILayout.Button("Create Ok & Cancel Button"))
        {
            GameObject buttons = new GameObject("Buttons", typeof(RectTransform), typeof(HorizontalLayoutGroup));
            buttons.GetComponent<RectTransform>().SetParent(trCommonPopup.transform);
            buttons.GetComponent<RectTransform>().localPosition = Vector3.zero;
            buttons.GetComponent<RectTransform>().localScale = Vector3.one;

            CreateButton(trCommonPopup.commonPopupResources.cancel_btn, buttons.transform);
            CreateButton(trCommonPopup.commonPopupResources.ok_btn, buttons.transform);
        }

        EditorGUILayout.Space();

        serializedObject.ApplyModifiedProperties();

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(trCommonPopup);
        }
    }

    public GameObject CreateButton(GameObject buttonObject, Transform transform)
    {
        GameObject button = Instantiate(buttonObject, transform);
        button.name = buttonObject.name;
        button.GetComponent<RectTransform>().localPosition = Vector3.zero;
        button.GetComponent<RectTransform>().localScale = Vector3.one;

        return button;
    }

    public void SetImage()
    {
        if (trCommonPopup.TryGetComponent(out UnityEngine.UI.Image image))
        {
            image.sprite = trCommonPopup.commonPopupResources?.backgroundResources[trCommonPopup.bgResource];
        }
    }
}