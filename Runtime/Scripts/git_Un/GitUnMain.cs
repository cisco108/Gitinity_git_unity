using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class GitUnMain
{
    private static IGitDiffReader _diffReader;
    private static PrefabSaver _saver;
    private static ITerminalInterface _terminal;
    private static ICommandBuilder _commandBuilder;
    // private static FileLocking _fileLocking;

    static GitUnMain()
    {
        _diffReader = new DiffGameObjectExtractor();
        _saver = new PrefabSaver();
        _terminal = new GitBashInterface();
        _commandBuilder = new GitBashCommandBuilder();

        // _fileLocking = new FileLocking(_terminal, _commandBuilder);
        
        GitUnGUI.OnStartSceneGet += Main;
        // GitUnGUI.OnLockFile += _fileLocking.LockFile;
    }

    private static void Main(string targetBranch, string sourceBranch)
    {
        WriteRelevantDiffToTxt(targetBranch, sourceBranch);
        SaveDiffObjectsAsPrefab(_diffReader.GetDiffObjects());
        PseudoMerge(targetBranch, sourceBranch);
    }


    private static void PseudoMerge(string targetBranch, string sourceBranch)
    {
        string commitCommand = _commandBuilder.GetCommit(FilePaths.diffPrefabsDirectory);
        _terminal.ExecuteBasicCommand(commitCommand);

        string switchCommand = _commandBuilder.GetSwitch(targetBranch);
        _terminal.ExecuteBasicCommand(switchCommand);

        string mergeCommand = _commandBuilder.GetMergeXours(sourceBranch);
        _terminal.ExecuteBasicCommand(mergeCommand);

        LogSystem.WriteLog(new string[] { commitCommand, switchCommand, mergeCommand });
    }

    private static void WriteRelevantDiffToTxt(string targetBranch, string sourceBranch)
    {
        string mergeBaseCommand = _commandBuilder.GetMergeBase(targetBranch, sourceBranch);
        string mergeBaseResult = _terminal.ExecuteResultToVar(mergeBaseCommand);

        string revParseCommand = _commandBuilder.GetRevParse(sourceBranch);
        string revParseResult = _terminal.ExecuteResultToVar(revParseCommand);


        string diffCommand = _commandBuilder.GetDiff(mergeBaseResult, revParseResult);
        _terminal.ExecuteResultToTxt(diffCommand);
        
        LogSystem.WriteLog(new string[] {mergeBaseCommand, mergeBaseResult, revParseCommand, revParseResult, diffCommand});
    }

    private static void SaveDiffObjectsAsPrefab(IList<GameObject> diffGaObjects)
    {
        foreach (var go in diffGaObjects)
        {
            _saver.CreatePrefab(go);
        }
    }
}