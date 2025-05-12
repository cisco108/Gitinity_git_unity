using UnityEditor;
using UnityEngine;

public class AssetVCS
{
    private string prefix;
    public AssetVCS()
    {
        Selection.selectionChanged += OnSelectionChanged;
        prefix = GlobalRefs.filePaths.versionControlledAssets; 
    }

    private void OnSelectionChanged()
    {
        Object selectedObj = Selection.activeObject;
        
        // string name = selectedObj.name;
        
        string path = AssetDatabase.GetAssetPath(selectedObj);
        
        Debug.Log("Asset selected: " + path);
        bool controlled = path.StartsWith(prefix);
        Debug.Log($"Asset is under AssetVCS? {controlled}");
    }
       
}