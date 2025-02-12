public class FileLocking
{
    private ITerminalInterface _terminal;
    private ICommandBuilder _commandBuilder;
    private const string subTreeBranchName = "file-locking-protocol";

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

        string subtreeCommand =
            _commandBuilder.GetSubtreeSplitNewBranch(FilePaths.lockingProtocolDirectory, subTreeBranchName);
        _terminal.ExecuteBasicCommand(subtreeCommand);

        LogSystem.WriteLog(new[] { mkdirCommand, touchCommand, subtreeCommand });
    }
}