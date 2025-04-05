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

        EditorSceneManager.sceneOpened += CheckIfFileIsLocked;
    }

    private void CheckIfFileIsLocked(Scene scene, OpenSceneMode mode)
    {
        string fetchCmd = GitCommands.fetch;
        _terminal.Execute(fetchCmd);

        string revParseFileLockBranchCmd = _commandBuilder.GetRevParse("origin/" + GlobalRefs.lockingBranch);
        string revParseHash = _terminal.ExecuteResultToString(revParseFileLockBranchCmd);

        string readLockFileCmd = _commandBuilder.GetCatFile(revParseHash, GlobalRefs.filePaths.lockedProtocolFile);
        string lockedFileContent = _terminal.ExecuteResultToString(readLockFileCmd);

        var result = _theLock.DeserializeFileLockInfo(lockedFileContent);
        string lockedFileName = result.lockedFile;
        string lockerEmail = result.personWhoLocked;


        if (scene.name == lockedFileName && lockerEmail != GlobalRefs.filePaths.userEmail)
        {
            string message = $"Access Violation!\n{lockedFileName} was locked by {lockerEmail}";
            Debug.LogError(message);
            OnFileIsLocked.Invoke(message);
            EditorSceneManager.OpenScene(GlobalRefs.openWhenSceneIsLocked);
        }
        else
        {
            Debug.Log($"This scene {scene.name} is save to work on, Congrats!");
        }

        LogSystem.WriteLog(new[]
        {
            fetchCmd, revParseFileLockBranchCmd, "hash of rev parse: ",
            revParseHash, readLockFileCmd, "locked file name: ", lockedFileName
        });
    }

    public void LockFile(string file)
    {
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
        
        Debug.LogError($"Unlocking is not done, but it would have unlocked: {file}");
    }
    
}