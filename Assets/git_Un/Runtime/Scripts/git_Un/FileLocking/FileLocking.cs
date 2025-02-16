using System.IO;

public class FileLocking
{
    private ITerminalInterface _terminal;
    private ICommandBuilder _commandBuilder;
    private const string subtreeBranchName = "file-locking-protocol";

    private string pathToLockFile = FilePaths.lockingProtocolDirectory + FilePaths.lockedProtocolFile;

    public FileLocking(ITerminalInterface terminal, ICommandBuilder commandBuilder)
    {
        _terminal = terminal;
        _commandBuilder = commandBuilder;

        SetupFileLockingSystemSubtree();
    }

    private void SetupFileLockingSystemSubtree()
    {
        string mkdirCommand = _commandBuilder.GetMkdir(FilePaths.lockingProtocolDirectory);
        _terminal.ExecuteBasicCommand(mkdirCommand);

        string touchCommand =
            _commandBuilder.GetTouch(FilePaths.lockingProtocolDirectory, FilePaths.lockedProtocolFile);
        _terminal.ExecuteBasicCommand(touchCommand);


        string commitCommand = _commandBuilder.GetCommit(FilePaths.lockingProtocolDirectory);
        _terminal.ExecuteBasicCommand(commitCommand);

        string subtreeCommand =
            _commandBuilder.GetSubtreeSplitNewBranch(FilePaths.lockingProtocolDirectory, subtreeBranchName);
        _terminal.ExecuteBasicCommand(subtreeCommand);

        LogSystem.WriteLog(new[] { mkdirCommand, touchCommand, commitCommand, subtreeCommand });
    }

    public void LockFile(string fileToLock)
    {
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