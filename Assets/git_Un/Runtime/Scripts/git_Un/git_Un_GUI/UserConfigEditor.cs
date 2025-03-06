using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor;

[CustomEditor(typeof(UserConfig), true)]
public class UserConfigEditor : Editor
{
    VisualElement _root;
    SerializedProperty _terminal;
    SerializedProperty _diffObjFolderName;
    SerializedProperty _remoteUrl;
    PropertyField _terminalField;
    PropertyField _diffObjFolderNameField;
    PropertyField _remoteUrlField;

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
        _diffObjFolderName = serializedObject.FindProperty(nameof(UserConfig.diffPrefabsDirName));
        _remoteUrl = serializedObject.FindProperty(nameof(UserConfig.remoteUrl));
    }

    void InitEditor()
    {
        _root = new VisualElement();
        _terminalField = new PropertyField(_terminal);
        _diffObjFolderNameField  = new PropertyField(_diffObjFolderName);
        _remoteUrlField = new PropertyField(_remoteUrl);
    }

    void Compose()
    {
        _root.Add(_terminalField);
        _root.Add(_diffObjFolderNameField);
        _root.Add(_remoteUrlField);
    }
}