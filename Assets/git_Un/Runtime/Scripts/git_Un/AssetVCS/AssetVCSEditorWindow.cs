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
    private bool _isCommitted;

    public event Action<string, string> OnUpdateVersion;
    public event Action<string, string> OnSaveChanges;

    public static void ShowWindow(string assetName, string[] versions, string path,
        Action<string, string> onUpdate, Action<string, string> onSave, string metadataInfo, bool isValid, bool isCommitted=true)
    {
        AssetVCSEditorWindow window = GetWindow<AssetVCSEditorWindow>("Asset VCS");
        window._assetName = assetName;
        window._versions = versions;
        window._pathOfContainedAsset = path;
        window.OnUpdateVersion = onUpdate;
        window.OnSaveChanges = onSave;
        window._metadataInfo = metadataInfo;
        window._isValid = isValid;
        window._isCommitted = isCommitted;
        window.minSize = new Vector2(480, 230);
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space(10);
        DrawHeader();
        EditorGUILayout.Space(10);

        using (new EditorGUILayout.VerticalScope("box"))
        {
            GUILayout.Label("Version Control", EditorStyles.boldLabel);
            DrawCommitStatus();
            
            if (_versions == null || _versions.Length == 0)
            {
                _versions = new[] { "No version yet" };
            }

            _selectedIndex = EditorGUILayout.Popup("Select Version", _selectedIndex, _versions);

            EditorGUILayout.Space(5);
            DrawActionButtons();
            EditorGUILayout.Space(10);
        }

        EditorGUILayout.Space(10);

        using (new EditorGUILayout.VerticalScope("box"))
        {
            GUILayout.Label("Asset Metadata", EditorStyles.boldLabel);
            var messageType = _isValid ? MessageType.Info : MessageType.Error;
            EditorGUILayout.HelpBox(_metadataInfo, messageType);
        }
    }

    private void DrawHeader()
    {
        GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 14,
            alignment = TextAnchor.MiddleCenter
        };

        GUILayout.Label($"Selected Asset: {_assetName}", headerStyle);
    }

    private void DrawActionButtons()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            // Switch Version Button
            bool shouldHighlightSwitch = _versions != null && _selectedIndex < _versions.Length;
            Color defaultColor = GUI.backgroundColor;

            if (shouldHighlightSwitch)
            {
                GUI.backgroundColor = new Color(0.4f, 0.7f, 1.0f); // Light blue
            }

            if (GUILayout.Button("Switch Version", GUILayout.Width(150), GUILayout.Height(25)))
            {
                OnUpdateVersion.Invoke(_versions[_selectedIndex], _pathOfContainedAsset);
            }

            GUI.backgroundColor = defaultColor;
        }

        using (new EditorGUILayout.HorizontalScope())
        {
            // Save Changes Button
            Color defaultColor = GUI.backgroundColor;

            if (!_isCommitted)
            {
                GUI.backgroundColor = new Color(0.3f, 0.9f, 0.3f); // Light green
            }

            if (GUILayout.Button("Save Changes", GUILayout.Width(150), GUILayout.Height(25)))
            {
                OnSaveChanges.Invoke(_versions[_selectedIndex], _pathOfContainedAsset);
            }

            GUI.backgroundColor = defaultColor;
        }
    }

    private void DrawCommitStatus()
    {
        GUIStyle statusStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            normal = {
                textColor = _isCommitted ? new Color(0.1f, 0.6f, 0.1f) : new Color(0.8f, 0.2f, 0.2f)
            }
        };

        string statusText = _isCommitted ? "Committed" : "Uncommitted";
        GUILayout.Label($"Status: {statusText}", statusStyle);
    }
}
