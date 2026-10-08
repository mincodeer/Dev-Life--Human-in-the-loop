using TMPro.EditorUtilities;
using UnityEditor;

// Preserve TMP's usual Inspector and expose our extra access configuration.
[CustomEditor(typeof(ProjectSetupDropdown))]
[CanEditMultipleObjects]
public class ProjectSetupDropdownEditor : DropdownEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("optionAccess"), true);
        serializedObject.ApplyModifiedProperties();
    }
}
