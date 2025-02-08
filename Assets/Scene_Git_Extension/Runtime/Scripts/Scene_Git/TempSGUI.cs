using System.Collections.Generic;
using UnityEngine;

public class TempSGUI : MonoBehaviour
{
    public SceneGitMain main = new();
    public IList<GameObject> diffObjects;

    public GameObject go;
    [Button("Save single")]
    private void Save()
    {
        main.SaveSingleObject(go);
    }

    [Button("Get Diff Objects")]
    private void GetDiffObjects()
    {
        Debug.Log("Hello");
        diffObjects = main.diffReader.GetDiffObjects();
        foreach (var go in diffObjects)
        {
           Debug.Log($"{go.name} transform: {go.transform}"); 
        }
        
        main.SaveDiffObjectsAsPrefab(diffObjects);
    }
}