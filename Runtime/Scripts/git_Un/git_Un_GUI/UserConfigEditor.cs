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
    SerializedProperty _diffObjFolderName;
    PropertyField _diffObjFolderNameField;
    SerializedProperty _remoteUrl;
    PropertyField _remoteUrlField;
    private SerializedProperty _fileToLock;
    private PropertyField _fileToLockField;

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
        _fileToLock = serializedObject.FindProperty(nameof(UserConfig.fileToLockNameOrPathLetsSee));
    }

    void InitEditor()
    {
        _root = new VisualElement();
        _terminalField = new PropertyField(_terminal);
        _diffObjFolderNameField  = new PropertyField(_diffObjFolderName);
        _remoteUrlField = new PropertyField(_remoteUrl);
        _fileToLockField = new PropertyField(_fileToLock);
    }

    void Compose()
    {
        _root.Add(_terminalField);
        _root.Add(_diffObjFolderNameField);
        _root.Add(_remoteUrlField);
        _root.Add(_fileToLockField);
    }
}