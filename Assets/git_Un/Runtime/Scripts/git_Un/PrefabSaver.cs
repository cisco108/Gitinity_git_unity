using System.IO;
using UnityEngine;
using UnityEditor;

public class PrefabSaver
{
    // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/PrefabUtility.SaveAsPrefabAsset.html
    public void CreatePrefab(GameObject gameObject)
    {
        if (!Directory.Exists("Assets/DiffObjects_as_Prefabs"))
            AssetDatabase.CreateFolder("Assets", "DiffObjects_as_Prefabs");
        string localPath = "Assets/DiffObjects_as_Prefabs/" + gameObject.name + ".prefab";

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