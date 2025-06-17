using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetVCSEditorWindow : EditorWindow
{
    private string _assetName;
    private string[] _versions;
    private int _selectedIndex = 0;
    private string _pathOfContainedAsset;
    private string _metadataInfo;
    private bool _isValid;

    public event Action<string, string> OnUpdateVersion;
    public event Action<string, string> OnSaveChanges;

    public static void ShowWindow(string assetName, string[] versions, string path,
        Action<string, string> onUpdate, Action<string, string> onSave, string metadataInfo, bool isValid)
    {
        AssetVCSEditorWindow window = GetWindow<AssetVCSEditorWindow>("Asset VCS");
        window._assetName = assetName;
        window._versions = versions;
        window._pathOfContainedAsset = path;
        window.OnUpdateVersion = onUpdate;
        window.OnSaveChanges = onSave;
        window._metadataInfo = metadataInfo;
        window._isValid = isValid;
        window.minSize = new Vector2(450, 180);
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label($"Version Selection: {_assetName}", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        if (_versions is null)
        {
            Debug.Log($"Asset: {_assetName} has no versions yet.");
            return;
        }
        _selectedIndex = EditorGUILayout.Popup("Select Version", _selectedIndex, _versions);

        if (GUILayout.Button("Switch Version"))
        {
            OnUpdateVersion.Invoke(_versions[_selectedIndex], _pathOfContainedAsset);
        }

        if (GUILayout.Button("Save Changes"))
        {
            OnSaveChanges.Invoke(_versions[_selectedIndex], _pathOfContainedAsset);
        }
        
        EditorGUILayout.Space();
        GUILayout.Label("Asset Metadata", EditorStyles.boldLabel);
        var messageType = _isValid ? MessageType.Info : MessageType.Error;
        EditorGUILayout.HelpBox(_metadataInfo, messageType);
    }

}
