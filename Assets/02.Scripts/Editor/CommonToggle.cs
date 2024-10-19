using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(CommonToggle))]
public class CommonToggleEditor : SelectableEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CommonToggle commonToggle = (CommonToggle)target;

        var type = (CommonToggle.ToggleType)EditorGUILayout.EnumPopup("Type", commonToggle.Type);

        if (type != commonToggle.Type)
        {
            commonToggle.Type = type;
            EditorUtility.SetDirty(commonToggle);
        }
    }
}