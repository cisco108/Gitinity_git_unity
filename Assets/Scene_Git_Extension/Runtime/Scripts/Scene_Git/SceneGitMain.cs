using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class SceneGitMain
{
    private static IGitDiffReader _diffReader;
    private static PrefabSaver _saver;
    private static ITerminalInterface _terminal;
    private static ICommandBuilder _commandBuilder;
    static SceneGitMain()
    {
        _diffReader = new DiffGameObjectExtractor();
        _saver = new PrefabSaver();
        _terminal = new GitBashInterface();
        _commandBuilder = new GitBashCommandBuilder();

        SceneGitGUI.OnStartSceneGet += StartSceneGet;
        SceneGitGUI.OnGetDiffFromSh += WriteRelevantDiffToTxt;
    }

    private static void StartSceneGet(string arg)
    {
       SaveDiffObjectsAsPrefab(_diffReader.GetDiffObjects()); 
    }
    


    public static void SaveSingleObject(GameObject go)
    {
        _saver.CreatePrefab(go);
    }

    public static void WriteRelevantDiffToTxt(string targetBranch, string sourceBranch)
    {
        // Debug.Log($"tagrget branch: {targetBranch} \n source branch: {sourceBranch}");
        _terminal.ExecuteResultToTxt("Hello friends");
        var s = _terminal.ExecuteResultToVar(GitCommands.log_oneline);
        Debug.Log(s);
    }

    public static void SaveDiffObjectsAsPrefab(IList<GameObject> diffGaObjects)
    {
        foreach (var go in diffGaObjects)
        {
            _saver.CreatePrefab(go);
        }
    }
}