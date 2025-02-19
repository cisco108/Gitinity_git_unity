using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor;

[CustomEditor(typeof(UserConfig), true)]
public class UserConfigEditor : Editor
{
    VisualElement _root;
    SerializedProperty _terminal;
    PropertyField _terminalField;

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
    }

    void InitEditor()
    {
        _root = new VisualElement();
        _terminalField = new PropertyField(_terminal);
    }

    void Compose()
    {
        _root.Add(_terminalField);
    }
}