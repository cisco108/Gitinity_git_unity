using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FileLocking
{
    private ICommandBuilder _commandBuilder;
    private ITerminalInterface _terminal;
    private TheLock _theLock;

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
        string lockedFileName = _theLock.DeserializeFileLockInfo(lockedFileContent);
        
        
        
        if (scene.name == lockedFileName)
        {
            Debug.LogError($"{lockedFileName} is locked! Ask your colleges what's up!");
        }
        else
        {
            Debug.Log($"This scene {scene.name} is save to work on, Congrats!");
        }
        
        LogSystem.WriteLog(new []
        {
            fetchCmd, revParseFileLockBranchCmd, "hash of rev parse: ", 
            revParseHash, readLockFileCmd, "locked file name: ", lockedFileName
        });
    }

    public void LockFile()
    {
        string saveBranchCmd = _commandBuilder.GetCurrentBranch();
        string currentBranch = _terminal.ExecuteResultToString(saveBranchCmd);

        string switchFileLockingCmd = _commandBuilder.GetSwitch(GlobalRefs.lockingBranch);
        _terminal.Execute(switchFileLockingCmd);

        /*
        string writeLockCmd = _commandBuilder.GetOverrideFileContent
            (GlobalRefs.filePaths.fileToLockNameOrPathLetsSee, GlobalRefs.filePaths.lockedProtocolFile);
        _terminal.Execute(writeLockCmd);
        */

        _theLock.WriteLocking();
        
        string commitCmd = _commandBuilder.GetCommit(" . ");
        _terminal.Execute(commitCmd);

        // string pushCmd = _commandBuilder.GetPush(); 
        // _terminal.Execute(pushCmd);

        string switchBackCmd = _commandBuilder.GetSwitch(currentBranch);
        _terminal.Execute(switchBackCmd);

        string pushAllCmd = _commandBuilder.GetPushAllBranches();
        _terminal.Execute(pushAllCmd);
        
        LogSystem.WriteLog(new []
        {
            saveBranchCmd, "current branch: ", currentBranch, switchFileLockingCmd, "locking here", commitCmd,
            pushAllCmd, switchBackCmd
        });
    }
}