using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGitMain
{
    public GitDiffReader diffReader = new DiffGameObjectExtractor();
    public PrefabSaver saver = new PrefabSaver();


    public void SaveSingleObject(GameObject go)
    {
        saver.CreatePrefab(go);
    }

    public void SaveDiffObjectsAsPrefab(IList<GameObject> diffGaObjects)
    {
        foreach (var go in diffGaObjects)
        {
            saver.CreatePrefab(go);
        }
    }
}