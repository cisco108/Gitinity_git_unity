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
            // here some data has to be read to see which versions are available
            // and then shown as selection for the asset
            // how to handle when they get commited? automatic on default the newest version 
            
            string getVersionsCmd = _commandBuilder.GetLogOfFile(path);
            string[] versions = _terminal.ExecuteResultToStringArr(getVersionsCmd);

            // string[] versions = new[] { "version2", "superVersion", "thisIsGreat" };
            string name = selectedObj.name;
            
            AssetVCSPopup popup = new AssetVCSPopup(name, versions, path);
            popup.OnUpdateVersion += UpdateVersion;
            PopupWindow.Show(new Rect(100, 100, 200, 100), popup);
            
            
        }
    }

    private void UpdateVersion(string versionCommit, string path)
    {
        string hash = versionCommit.Remove(7);

        string checkoutCmd = _commandBuilder.GetCheckout(hash, path);
        Debug.Log(checkoutCmd);
        _terminal.Execute(checkoutCmd);
    }
       
}


public class AssetVCSPopup : PopupWindowContent
{
    private readonly string _assetName;
    private string[] _versions;
    private int _selectedIndex = 0;
    private string _pathOfContainedAsset;

    public event Action<string, string> OnUpdateVersion;
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
        if (GUILayout.Button("Change Version"))
        {
            OnUpdateVersion.Invoke(_versions[_selectedIndex], _pathOfContainedAsset);
        }
        
        EditorGUILayout.Space();
        if (GUILayout.Button("Close"))
        {
            editorWindow.Close();
        }
    }
}