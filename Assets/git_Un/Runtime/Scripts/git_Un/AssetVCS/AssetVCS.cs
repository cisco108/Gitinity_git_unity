using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class AssetVCS
{
    private string prefix;
    private ICommandBuilder _commandBuilder;
    private ITerminalInterface _terminal;
    public AssetVCS(ITerminalInterface terminal, ICommandBuilder commandBuilder)
    {
        _terminal = terminal;
        _commandBuilder = commandBuilder;
        
        Selection.selectionChanged += OnSelectionChanged;
        prefix = GlobalRefs.filePaths.versionControlledAssets; 
    }

    private void OnSelectionChanged()
    {
        Object selectedObj = Selection.activeObject;
        
        
        string path = AssetDatabase.GetAssetPath(selectedObj);
        
        Debug.Log("Asset selected: " + path);
        bool controlled = path.StartsWith(prefix);
        Debug.Log($"Asset is under AssetVCS? {controlled}");

        if (controlled)
        {
            string getVersionsCmd = _commandBuilder.GetLogOfFile(path);
            string[] versions = _terminal.ExecuteResultToStringArr(getVersionsCmd);

            // string[] versions = new[] { "version2", "superVersion", "thisIsGreat" };
            string name = selectedObj.name;
            
            AssetVCSEditorWindow.ShowWindow(name, versions, path, UpdateVersion, SaveChanges);
 
            /*
            AssetVCSPopup popup = new AssetVCSPopup(name, versions, path);
            popup.OnUpdateVersion += UpdateVersion;
            popup.OnSaveChanges += SaveChanges;
            PopupWindow.Show(new Rect(100, 100, 200, 100), popup);
            */
            
            
        }
    }

    private void UpdateVersion(string versionCommit, string path)
    {
        string hash = versionCommit.Remove(7);

        string checkoutCmd = _commandBuilder.GetCheckout(hash, path);
        Debug.Log(checkoutCmd);
        _terminal.Execute(checkoutCmd);
        AssetDatabase.Refresh();
    }

    private void SaveChanges(string versionCommit, string path)
    {
         string hash = versionCommit.Remove(7);

         string commitCmd = _commandBuilder.GetCommit(path, $"Changed {path} to version {hash}");
         Debug.Log(commitCmd);
         _terminal.Execute(commitCmd);       
    }
       
}



public class AssetVCSEditorWindow : EditorWindow
{
    private string _assetName;
    private string[] _versions;
    private int _selectedIndex = 0;
    private string _pathOfContainedAsset;

    public event Action<string, string> OnUpdateVersion;
    public event Action<string, string> OnSaveChanges;

    public static void ShowWindow(string assetName, string[] versions, string path,
        Action<string, string> onUpdate, Action<string, string> onSave)
    {
        AssetVCSEditorWindow window = GetWindow<AssetVCSEditorWindow>("Asset VCS");
        window._assetName = assetName;
        window._versions = versions;
        window._pathOfContainedAsset = path;
        window.OnUpdateVersion = onUpdate;
        window.OnSaveChanges = onSave;
        window.minSize = new Vector2(450, 180);
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label($"Version Selection: {_assetName}", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        _selectedIndex = EditorGUILayout.Popup("Select Version", _selectedIndex, _versions);

        if (GUILayout.Button("Switch Version"))
        {
            OnUpdateVersion.Invoke(_versions[_selectedIndex], _pathOfContainedAsset);
        }

        if (GUILayout.Button("Save Changes"))
        {
            OnSaveChanges.Invoke(_versions[_selectedIndex], _pathOfContainedAsset);
        }
    }
}








public class AssetVCSPopup : PopupWindowContent
{
    private readonly string _assetName;
    private string[] _versions;
    private int _selectedIndex = 0;
    private string _pathOfContainedAsset;

    public event Action<string, string> OnUpdateVersion;
    public event Action<string, string> OnSaveChanges;
    public AssetVCSPopup(string name, string[] versions, string path)
    {
        _assetName = name;
        _versions = versions;
        _pathOfContainedAsset = path;
    }
    public override Vector2 GetWindowSize() => new Vector2(450, 180);

    public override void OnGUI(Rect rect)
    {
        GUILayout.Label($"Version Selection: {_assetName}", EditorStyles.boldLabel);
        
        EditorGUILayout.Space();

        // Dropdown selection for versions
        _selectedIndex = EditorGUILayout.Popup("Select Version", _selectedIndex, _versions);
        if (GUILayout.Button("Switch Version"))
        {
            OnUpdateVersion.Invoke(_versions[_selectedIndex], _pathOfContainedAsset);
        }
        
        if (GUILayout.Button("Safe Changes"))
        {
            OnSaveChanges.Invoke(_versions[_selectedIndex], _pathOfContainedAsset);
        }
        
        
        
        EditorGUILayout.Space();
        if (GUILayout.Button("Close"))
        {
            editorWindow.Close();
        }
    }
}