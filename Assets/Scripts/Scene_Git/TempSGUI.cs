using System.Collections.Generic;
using UnityEngine;

public class TempSGUI : MonoBehaviour
{
    public SceneGitMain main = new();

    public IList<GameObject> diffObjects;

    [Button("Get Diff Objects")]
    private void GetDiffObjects()
    {
        Debug.Log("Hello");
        diffObjects = main.diffReader.GetDiffObjects();
        foreach (var go in diffObjects)
        {
           Debug.Log($"{go.name} transform: {go.transform}"); 
        }
    }
}