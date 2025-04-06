using System;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FileLocking
{
    private ICommandBuilder _commandBuilder;
    private ITerminalInterface _terminal;
    private TheLock _theLock;

    public static event Action<string> OnFileIsLocked;

    public void TestEvent()
    {
        // OnFileIsLocked.Invoke($"Access Violation! \nFile was locked by Name");
        // EditorSceneManager.OpenScene("Assets/Scenes/SampleScene.unity");
        EditorSceneManager.OpenScene("Assets/git_Un/Runtime/Scene/OpensWhenSceneIsLocked.unity");
    }

    public FileLocking()
    {
        Debug.LogWarning("You are using empty FileLocking");
    } // just for testing

    public FileLocking(ITerminalInterface terminal, ICommandBuilder commandBuilder)
    {
        _terminal = terminal;
        _commandBuilder = commandBuilder;
        _theLock = new TheLock();

        EditorSceneManager.sceneOpened += FeedbackIfLocked;
    }

    private (bool isLocked, string whoLockedIt) CheckIfFileIsLocked(string file)
    {
        /*string fetchCmd = GitCommands.fetch;
        _terminal.Execute(fetchCmd);

        string revParseFileLockBranchCmd = _commandBuilder.GetRevParse("origin/" + GlobalRefs.lockingBranch);
        string revParseHash = _terminal.ExecuteResultToString(revParseFileLockBranchCmd);

        string readLockFileCmd = _commandBuilder.GetCatFile(revParseHash, GlobalRefs.filePaths.lockedProtocolFile);
        string lockedFileContentJson = _terminal.ExecuteResultToString(readLockFileCmd);*/


        string readLockFileCmd = _commandBuilder.GetReadLockedFile();
        string lockedFileContentJson = _terminal.ExecuteResultToString(readLockFileCmd);
        // git cat-file -p origin/file-locking:locked_files.json
        LogSystem.WriteLog(new[] { readLockFileCmd, "locked file content:", lockedFileContentJson });

        /*LogSystem.WriteLog(new[]
        {
            "CheckIfFileIsLocked: ", fetchCmd, revParseFileLockBranchCmd, "hash of rev parse: ",
            revParseHash, readLockFileCmd, "locked file content: ", lockedFileContentJson
        });*/

        bool isLocked = _theLock.IsFileLocked(lockedFileContentJson, file, out var whoLockedIt);
        return (isLocked, whoLockedIt);
    }

    private void FeedbackIfLocked(Scene scene, OpenSceneMode _)
    {
        if (!GlobalRefs.filePaths.useFileLocking) return;

        (bool isLocked, string whoLockedIt) = CheckIfFileIsLocked(scene.name);

        if (isLocked && whoLockedIt != GlobalRefs.filePaths.userEmail)
        {
            string message = $"Access Violation!\n{scene.name} was locked by {whoLockedIt}";
            Debug.LogError(message);
            OnFileIsLocked.Invoke(message);
        }
        else
        {
            Debug.Log($"This scene {scene.name} is save to work on, Congrats!");
        }
    }

    public void LockFile(string file)
    {
        if (!GlobalRefs.filePaths.useFileLocking) return;

        (bool isFileAlreadyLocked, string whoLockedIt) = CheckIfFileIsLocked(file);
        if (isFileAlreadyLocked)
        {
            Debug.LogWarning($"{file} was already locked by {whoLockedIt}");
            return;
        }

        string saveBranchCmd = _commandBuilder.GetCurrentBranch();
        string currentBranch = _terminal.ExecuteResultToString(saveBranchCmd);

        string switchFileLockingCmd = _commandBuilder.GetSwitch(GlobalRefs.lockingBranch);
        _terminal.Execute(switchFileLockingCmd);

        _theLock.WriteLocking(file);

        string commitCmd = _commandBuilder.GetCommit(" . ");
        _terminal.Execute(commitCmd);

        // string pushCmd = _commandBuilder.GetPush(); 
        // _terminal.Execute(pushCmd);

        string switchBackCmd = _commandBuilder.GetSwitch(currentBranch);
        _terminal.Execute(switchBackCmd);

        string pushAllCmd = _commandBuilder.GetPushAllBranches();
        _terminal.Execute(pushAllCmd);

        LogSystem.WriteLog(new[]
        {
            saveBranchCmd, "current branch: ", currentBranch, switchFileLockingCmd, "locking here", commitCmd,
            pushAllCmd, switchBackCmd
        });
    }

    public void UnlockFile(string file)
    {
        if (!GlobalRefs.filePaths.useFileLocking) return;

        string saveBranchCmd = _commandBuilder.GetCurrentBranch();
        string currentBranch = _terminal.ExecuteResultToString(saveBranchCmd);

        string switchFileLockingCmd = _commandBuilder.GetSwitch(GlobalRefs.lockingBranch);
        _terminal.Execute(switchFileLockingCmd);

        _theLock.WriteUnlocking(file);

        string commitCmd = _commandBuilder.GetCommit(" . ");
        _terminal.Execute(commitCmd);

        // string pushCmd = _commandBuilder.GetPush(); 
        // _terminal.Execute(pushCmd);

        string switchBackCmd = _commandBuilder.GetSwitch(currentBranch);
        _terminal.Execute(switchBackCmd);

        string pushAllCmd = _commandBuilder.GetPushAllBranches();
        _terminal.Execute(pushAllCmd);

        LogSystem.WriteLog(new[]
        {
            saveBranchCmd, "current branch: ", currentBranch, switchFileLockingCmd, "unlocking here", commitCmd,
            pushAllCmd, switchBackCmd
        });
    }
}