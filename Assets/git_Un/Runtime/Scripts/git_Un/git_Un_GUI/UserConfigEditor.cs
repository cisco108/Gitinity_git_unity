using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor;

[CustomEditor(typeof(UserConfig), true)]
public class UserConfigEditor : Editor
{
    VisualElement _root;
    SerializedProperty _terminal;
    SerializedProperty _diffObjectLocation;
    PropertyField _terminalField;
    PropertyField _diffObjectLocField;

    public override VisualElement CreateInspectorGUI()
    {
        FindProperties();
        InitEditor();
        Compose();
        return _root;
    }

    void FindProperties()
    {
        _terminal = serializedObject.FindProperty(nameof(UserConfig.gitBashExe));
        _diffObjectLocation = serializedObject.FindProperty(nameof(UserConfig.diffPrefabsDirectory));
    }

    void InitEditor()
    {
        _root = new VisualElement();
        _terminalField = new PropertyField(_terminal);
        _diffObjectLocField  = new PropertyField(_diffObjectLocation);
    }

    void Compose()
    {
        _root.Add(_terminalField);
        _root.Add(_diffObjectLocField);
    }
}