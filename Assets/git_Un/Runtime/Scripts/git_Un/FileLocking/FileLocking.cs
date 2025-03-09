using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FileLocking
{
    private ICommandBuilder _commandBuilder;
    private ITerminalInterface _terminal;
    public FileLocking(ITerminalInterface terminal, ICommandBuilder commandBuilder)
    {
        _terminal = terminal;
        _commandBuilder = commandBuilder;

        EditorSceneManager.sceneOpened += CheckIfFileIsLocked;
    }

    private void CheckIfFileIsLocked(Scene scene, OpenSceneMode mode)
    {
        string fetchCmd = GitCommands.fetch;
        _terminal.Execute(fetchCmd);

        string revParseFileLockBranchCmd = _commandBuilder.GetRevParse("origin/" + GlobalRefs.lockingBranch);
        string revParseHash = _terminal.ExecuteResultToString(revParseFileLockBranchCmd);

        string readLockFileCmd = _commandBuilder.GetCatFile(revParseHash, GlobalRefs.filePaths.lockedProtocolFile);
        string lockedFileName = _terminal.ExecuteResultToString(readLockFileCmd);
        
        if (scene.name == lockedFileName)
        {
            Debug.LogError($"{lockedFileName} is locked! Ask your colleges what's up!");
        }
        else
        {
            Debug.Log($"This scene is save to work on, Congrats!");
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

        string writeLockCmd = _commandBuilder.GetEcho(GlobalRefs.filePaths.fileToLockNameOrPathLetsSee);
        _terminal.ExecuteResultToTxt(writeLockCmd, GlobalRefs.filePaths.lockedProtocolFile);

        string commitCmd = _commandBuilder.GetCommit(" . ");
        _terminal.Execute(commitCmd);

        string pushCmd = _commandBuilder.GetPush(); 
        _terminal.Execute(pushCmd);

        string switchBackCmd = _commandBuilder.GetSwitch(currentBranch);
        _terminal.Execute(switchBackCmd);
        
        LogSystem.WriteLog(new []
        {
            saveBranchCmd, "current branch: ", currentBranch, switchFileLockingCmd, writeLockCmd, commitCmd,
            pushCmd, switchBackCmd
        });
    }
}