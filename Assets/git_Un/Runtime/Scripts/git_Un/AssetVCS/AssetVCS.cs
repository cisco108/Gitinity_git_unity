using UnityEditor;
using UnityEngine;

public class AssetVCS
{
    private string prefix;
    private ICommandBuilder _commandBuilder;
    private ITerminalInterface _terminal;
    public AssetVCS()
    {
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
            
            PopupWindow.Show(new Rect(100, 100, 200, 100), new AssetVCSPopup(name, versions));
            
        }
    }
       
}


public class AssetVCSPopup : PopupWindowContent
{
    private readonly string _assetName;
    private string[] _versions;
    private int _selectedIndex = 0;
    public AssetVCSPopup(string name, string[] versions)
    {
        _assetName = name;
        _versions = versions;
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
            editorWindow.Close();
        }
    }
}