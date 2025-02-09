using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class SceneGitMain
{
    private static GitDiffReader _diffReader;
    private static PrefabSaver _saver;
    private static ITerminalInterface _terminal;
    static SceneGitMain()
    {
        _diffReader = new DiffGameObjectExtractor();
        _saver = new PrefabSaver();
        _terminal = new GitBashInterface();

        SceneGitGUI.OnStartSceneGet += StartSceneGet;
        SceneGitGUI.OnGetDiffFromSh += GetDiff;
    }

    private static void StartSceneGet()
    {
       SaveDiffObjectsAsPrefab(_diffReader.GetDiffObjects()); 
    }
    


    public static void SaveSingleObject(GameObject go)
    {
        _saver.CreatePrefab(go);
    }

    public static void GetDiff()
    {
        _terminal.Execute();
    }

    public static void SaveDiffObjectsAsPrefab(IList<GameObject> diffGaObjects)
    {
        foreach (var go in diffGaObjects)
        {
            _saver.CreatePrefab(go);
        }
    }
}