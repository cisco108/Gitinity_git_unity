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

        SceneGitGUI.OnStartSceneGet += Main;
        // SceneGitGUI.OnGetDiffFromSh += WriteRelevantDiffToTxt;
    }

    private static void Main(string targetBranch, string sourceBranch)
    {
        WriteRelevantDiffToTxt(targetBranch, sourceBranch);
        SaveDiffObjectsAsPrefab(_diffReader.GetDiffObjects());
        PseudoMerge();
    }


    private static void PseudoMerge()
    {
        string mergeCommand = _commandBuilder.GetCommit(FilePaths.diffPrefabsDirectory);
        _terminal.ExecuteCommit(mergeCommand);
    }

    private static void WriteRelevantDiffToTxt(string targetBranch, string sourceBranch)
    {
        string mergeBaseCommand = _commandBuilder.GetMergeBase(targetBranch, sourceBranch);
        string mergeBaseResult = _terminal.ExecuteResultToVar(mergeBaseCommand);

        string revParseCommand = _commandBuilder.GetRevParse(sourceBranch);
        string revParseResult = _terminal.ExecuteResultToVar(revParseCommand);


        string diffCommand = _commandBuilder.GetDiff(mergeBaseResult, revParseResult);
        Debug.Log(diffCommand);
        _terminal.ExecuteResultToTxt(diffCommand);
    }

    private static void SaveDiffObjectsAsPrefab(IList<GameObject> diffGaObjects)
    {
        foreach (var go in diffGaObjects)
        {
            _saver.CreatePrefab(go);
        }
    }
}