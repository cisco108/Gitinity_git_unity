using System.IO;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class PrefabSaver
{
    public void CreatePrefab(GameObject gameObject)
    {
        if (!Directory.Exists("Assets/DiffObjects_as_Prefabs"))
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        string localPath = "Assets/Prefabs/" + gameObject.name + ".prefab";

        // Make sure the file name is unique, in case an existing Prefab has the same name.
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        // Create the new Prefab and log whether Prefab was saved successfully.
        bool prefabSuccess;
        PrefabUtility.SaveAsPrefabAsset(gameObject, localPath, out prefabSuccess);
        if (prefabSuccess == true)
            Debug.Log("Prefab was saved successfully");
        else
            Debug.Log("Prefab failed to save" + prefabSuccess);
    }
}