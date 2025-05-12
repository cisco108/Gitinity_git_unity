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
        
        
        string path = AssetDatabase.GetAssetPath(selectedObj);
        
        Debug.Log("Asset selected: " + path);
        bool controlled = path.StartsWith(prefix);
        Debug.Log($"Asset is under AssetVCS? {controlled}");


        if (controlled)
        {
            string name = selectedObj.name;
            PopupWindow.Show(new Rect(100, 100, 200, 100), new AssetVCSPopup(name));
            // here some data has to be read to see which versions are available
            // and then shown as selection for the asset
            // how to handle when they get commited? automatic on default the newest version 
            
        }
    }
       
}


public class AssetVCSPopup : PopupWindowContent
{
    private string assetName;
    public AssetVCSPopup(string name)
    {
        assetName = name;
    }
    public override Vector2 GetWindowSize() => new Vector2(450, 180);

    public override void OnGUI(Rect rect)
    {
        GUILayout.Label($"Version Selection: {assetName}", EditorStyles.boldLabel);
        if (GUILayout.Button("OK"))
        {
            editorWindow.Close();
        }
    }
}