﻿using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class GitUnMain
{
    private static IGitDiffReader _diffReader;
    private static PrefabSaver _saver;
    private static ITerminalInterface _terminal;
    private static ICommandBuilder _commandBuilder;
    private static FileLocking _fileLocking;
    private static AssetVCS _assetVCS;
    private static MRManager _mrManager;

    static GitUnMain()
    {
        _diffReader = new DiffGameObjectExtractor();
        _saver = new PrefabSaver();
        _terminal = new GitBashInterface();
        _commandBuilder = new GitBashCommandBuilder();

        _mrManager = new MRManager(_terminal, _commandBuilder);

        _fileLocking = new FileLocking(_terminal, _commandBuilder);


        GitinityUI.OnSetup += SetupGitinity;
        GitinityUI.OnGetGitInfo += OnGetGitData;
        GitinityUI.OnGetFeatureInfo += _mrManager.GetFeatureInfo;
        GitinityUI.OnStartFeature += _mrManager.StartFeature;
        GitinityUI.OnMerge += Main;
        GitinityUI.OnLockFile += _fileLocking.LockFile;
        GitinityUI.OnUnlockFile += _fileLocking.UnlockFile;
        GitinityUI.OnActivateAssetVCS += AssetVCSSetActive;
        
        OnGetGitData();
        // for testing in development project:
        // AssetVCSSetActive(true);
    }
    
    private static void AssetVCSSetActive(bool state)
    {
        if (_assetVCS is null)
        {
            _assetVCS = new AssetVCS(_terminal, _commandBuilder);
        }

        string path = GlobalRefs.filePaths.versionControlledAssets;
        if (!AssetDatabase.IsValidFolder(path))
        {
            Directory.CreateDirectory(path);
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<AssetValidationSettings>(), path + "Rules.asset");
        }

        
        _assetVCS.isActive = state;
    }

    private static void SetupGitinity()
    {
        string sh1 = File.ReadAllText(GlobalRefs.ShellScript(1));
        _terminal.Execute(sh1);
       
        string addToGitignoreCmd = _commandBuilder.GetWriteLinesToFile(
            new[] { GlobalRefs.filePaths.logsFile, GlobalRefs.filePaths.lockedProtocolFile }, GlobalRefs.gitignore);
        _terminal.Execute(addToGitignoreCmd);

        string sh2 = File.ReadAllText(GlobalRefs.ShellScript(2));
        _terminal.Execute(sh2);
      
        string newIgnore = _commandBuilder.GetOverrideFileContent
            ($"'*\\n!{GlobalRefs.filePaths.lockedProtocolFile}'", GlobalRefs.gitignore);
        _terminal.Execute(newIgnore);

        string commitCmd = _commandBuilder.GetCommit(GlobalRefs.gitignore);
        _terminal.Execute(commitCmd);

        string switch2Cmd = _commandBuilder.GetSwitch(GlobalRefs.filePaths.defaultBranchName);
        _terminal.Execute(switch2Cmd);

        string commit2Cmd = _commandBuilder.GetCommit(".");
        _terminal.Execute(commit2Cmd);

        string addRemoteCmd = _commandBuilder.GetAddRemote();
        _terminal.Execute(addRemoteCmd);

        string pushAllCmd = _commandBuilder.GetPushAllBranches();
        _terminal.Execute(pushAllCmd);
        
        string pathToSetupScript = GlobalRefs.ShellScript(3);
        string setHooksCmd = $"cp {pathToSetupScript} . && ./{GlobalRefs.filePaths.setupGitHookFile}";
        _terminal.Execute(setHooksCmd);
        
        LogSystem.WriteLog(new[]
        {
            sh1, addToGitignoreCmd, sh2, commitCmd,
            newIgnore, commitCmd, switch2Cmd, commit2Cmd, addRemoteCmd, pushAllCmd, setHooksCmd
        });
    }

    private static void OnGetGitData()
    {
        var branches = _terminal.ExecuteResultToStringArr(GitCommands.branch);
        GlobalRefs.SetState(branches);
    }

    private static void Main(string targetBranch, string sourceBranch)
    {
        WriteRelevantDiffToTxt(targetBranch, sourceBranch);
        SaveDiffObjectsAsPrefab(_diffReader.GetDiffObjects());
        PseudoMerge(targetBranch, sourceBranch);
    }


    private static void PseudoMerge(string targetBranch, string sourceBranch)
    {
        string commitCommand = _commandBuilder.GetCommit(GlobalRefs.filePaths.DiffPrefabsDirectory);
        _terminal.Execute(commitCommand);

        string switchCommand = _commandBuilder.GetSwitch(targetBranch);
        _terminal.Execute(switchCommand);

        string mergeCommand = _commandBuilder.GetMergeXours(sourceBranch);
        _terminal.Execute(mergeCommand);

        LogSystem.WriteLog(new string[] { commitCommand, switchCommand, mergeCommand });
    }

    private static void WriteRelevantDiffToTxt(string targetBranch, string sourceBranch)
    {
        string mergeBaseCommand = _commandBuilder.GetMergeBase(targetBranch, sourceBranch);
        string mergeBaseResult = _terminal.ExecuteResultToString(mergeBaseCommand);

        string revParseCommand = _commandBuilder.GetRevParse(sourceBranch);
        string revParseResult = _terminal.ExecuteResultToString(revParseCommand);


        string diffCommand = _commandBuilder.GetDiff(mergeBaseResult, revParseResult);
        _terminal.ExecuteResultToTxt(diffCommand);

        LogSystem.WriteLog(new string[]
            { mergeBaseCommand, mergeBaseResult, revParseCommand, revParseResult, diffCommand });
    }

    private static void SaveDiffObjectsAsPrefab(IList<GameObject> diffGaObjects)
    {
        foreach (var go in diffGaObjects)
        {
            _saver.CreatePrefab(go);
        }
    }
}