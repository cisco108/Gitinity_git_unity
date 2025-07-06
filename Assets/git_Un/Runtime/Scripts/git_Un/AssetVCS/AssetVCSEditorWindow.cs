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
        Action<string, string> onUpdate, Action<string, string> onSave, string metadataInfo, bool isValid,
        bool isCommitted = true)
    {
        AssetVCSEditorWindow window = GetWindow<AssetVCSEditorWindow>("Asset VCS & Validation");
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
            // DrawCommitStatus();

            if (_versions == null || _versions.Length == 0)
            {
                _versions = new[] { "No version yet" };
            }

            _selectedIndex = EditorGUILayout.Popup("Select Version", _selectedIndex, _versions);

            EditorGUILayout.Space(5);
            DrawActionButtons();
            EditorGUILayout.Space(10);
        }

        DrawValidationState();

        EditorGUILayout.Space(10);
    }

    /*
    void DrawValidationState()
    {
        using (new EditorGUILayout.VerticalScope("box"))
        {
            GUILayout.Label("Asset Metadata", EditorStyles.boldLabel);
            var messageType = _isValid ? MessageType.Info : MessageType.Error;
            EditorGUILayout.HelpBox(_metadataInfo, messageType);
        }
    }
        */


    void DrawValidationState()
    {
        GUILayout.Space(20);
        
        GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 15,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = _isValid ? new Color(0.2f, 1f, 0.6f) : new Color(1f, 0.3f, 0.3f) }
        };

        GUIStyle boxStyle = new GUIStyle(GUI.skin.box)
        {
            padding = new RectOffset(12, 12, 12, 12),
            margin = new RectOffset(10, 10, 10, 10),
            normal = { background = Texture2D.grayTexture }
        };

        GUILayout.BeginVertical(boxStyle);

        GUILayout.Space(6);
        string status = _isValid ? "Okay" : "Failed"; 
        GUILayout.Label($"Asset Validation Status: {status}", headerStyle);
        GUILayout.Space(6);

        var messageType = _isValid ? MessageType.Info : MessageType.Error;

        EditorGUILayout.HelpBox(_metadataInfo, messageType);

        GUILayout.Space(6);
        GUILayout.EndVertical();
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
            bool shouldHighlightSwitch = _versions != null && _selectedIndex < _versions.Length;
            Color defaultColor = GUI.backgroundColor;

            if (shouldHighlightSwitch)
            {
                GUI.backgroundColor = new Color(0.4f, 0.7f, 1.0f); // Light blue
            }

            if (GUILayout.Button("Switch Version", GUILayout.Width(150), GUILayout.Height(25)))
            {
                OnUpdateVersion.Invoke(_versions[_selectedIndex], _pathOfContainedAsset);
                _isCommitted = false;
            }

            GUI.backgroundColor = defaultColor;
        }

        using (new EditorGUILayout.HorizontalScope())
        {
            Color defaultColor = GUI.backgroundColor;

            if (!_isCommitted)
            {
                GUI.backgroundColor = new Color(0.3f, 0.9f, 0.3f); // Light green
            }

            if (GUILayout.Button("Save Changes", GUILayout.Width(150), GUILayout.Height(25)))
            {
                OnSaveChanges.Invoke(_versions[_selectedIndex], _pathOfContainedAsset);
                _isCommitted = true;
            }

            GUI.backgroundColor = defaultColor;
        }
    }

    private void DrawCommitStatus()
    {
        GUIStyle statusStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            normal =
            {
                textColor = _isCommitted ? new Color(0.1f, 0.6f, 0.1f) : new Color(0.8f, 0.2f, 0.2f)
            }
        };

        string statusText = _isCommitted ? "Committed" : "Uncommitted";
        GUILayout.Label($"Status: {statusText}", statusStyle);
    }
}