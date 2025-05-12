using UnityEditor;
using UnityEngine;

public class TestAssetsEvents : MonoBehaviour
{

    void Start()
    {
        Selection.selectionChanged += OnSelectionChanged;
    }

    private static void OnSelectionChanged()
    {
        // Check if the selected object is an asset in the Project window
        Object selected = Selection.activeObject;
        if (selected != null)
        {
            string path = AssetDatabase.GetAssetPath(selected);
            if (!string.IsNullOrEmpty(path) && !AssetDatabase.IsValidFolder(path))
            {
                Debug.Log("Asset selected: " + path);
                // Place your custom logic here
            }
        }
    }
        
}