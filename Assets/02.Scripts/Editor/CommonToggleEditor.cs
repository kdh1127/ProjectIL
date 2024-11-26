using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.AnimatedValues;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(CommonToggle))]
public class CommonToggleEditor : SelectableEditor
{
    SerializedProperty toggleType;

    private new void OnEnable()
	{
        base.OnEnable();
        toggleType = serializedObject.FindProperty("type");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.PropertyField(toggleType);

		serializedObject.ApplyModifiedProperties();
    }

}