using System.IO;

public class FileLocking
{
    private ITerminalInterface _terminal;
    private ICommandBuilder _commandBuilder;
    private const string subtreeBranchName = "file-locking-protocol";

    private string pathToLockFile = GlobalRefs.filePaths.lockingProtocolDirectory + GlobalRefs.filePaths.lockedProtocolFile;

    public FileLocking(ITerminalInterface terminal, ICommandBuilder commandBuilder)
    {
        _terminal = terminal;
        _commandBuilder = commandBuilder;
    }

    private void SetupFileLockingSystemSubtree()
    {
        string mkdirCommand = _commandBuilder.GetMkdir(GlobalRefs.filePaths.lockingProtocolDirectory);
        _terminal.ExecuteBasicCommand(mkdirCommand);

        string touchCommand =
            _commandBuilder.GetTouch(GlobalRefs.filePaths.lockingProtocolDirectory,GlobalRefs.filePaths.lockedProtocolFile);
        _terminal.ExecuteBasicCommand(touchCommand);


        string commitCommand = _commandBuilder.GetCommit(GlobalRefs.filePaths.lockingProtocolDirectory);
        _terminal.ExecuteBasicCommand(commitCommand);

        string subtreeCommand =
            _commandBuilder.GetSubtreeSplitNewBranch(GlobalRefs.filePaths.lockingProtocolDirectory, subtreeBranchName);
        _terminal.ExecuteBasicCommand(subtreeCommand);

        LogSystem.WriteLog(new[] { mkdirCommand, touchCommand, commitCommand, subtreeCommand });
    }

    public void LockFile(string fileToLock)
    {
        SetupFileLockingSystemSubtree();
        string switchCommand = _commandBuilder.GetSwitch(subtreeBranchName);
        _terminal.ExecuteBasicCommand(switchCommand);

        File.AppendAllLines(pathToLockFile, new[] { fileToLock });

        string commitCommand = _commandBuilder.GetCommit(" .");
        _terminal.ExecuteBasicCommand(commitCommand);

        //TODO:  make modular
        string switchBackCommand = _commandBuilder.GetSwitch("master");
        _terminal.ExecuteBasicCommand(switchBackCommand);

        string pushCommand = _commandBuilder.GetPush(subtreeBranchName);
        _terminal.ExecuteBasicCommand(pushCommand);

        LogSystem.WriteLog(new[] { switchCommand, commitCommand, switchBackCommand, pushCommand });
    }
}