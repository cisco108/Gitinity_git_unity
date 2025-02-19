using System.IO;
using UnityEngine;
using UnityEditor;

public class PrefabSaver
{
    private UserConfig filePaths = GlobalRefs.filePaths; 
    // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/PrefabUtility.SaveAsPrefabAsset.html
    public void CreatePrefab(GameObject gameObject)
    {
        if (!Directory.Exists(filePaths.DiffPrefabsDirectory))
        {
            AssetDatabase.CreateFolder(filePaths.diffPrefabsParentDirectory, filePaths.diffPrefabsDirName);
        }

        string localPath = filePaths.DiffPrefabsDirectory + gameObject.name + ".prefab";

        // Make sure the file name is unique, in case an existing Prefab has the same name.
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        // Create the new Prefab and log whether Prefab was saved successfully.
        bool prefabSuccess;
        PrefabUtility.SaveAsPrefabAsset(gameObject, localPath, out prefabSuccess);
        if (prefabSuccess)
            Debug.Log("Prefab was saved successfully");
        else
            Debug.Log("Prefab failed to save" + prefabSuccess);
    }
}