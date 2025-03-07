public class FileLocking
{
    private ICommandBuilder _commandBuilder;
    private ITerminalInterface _terminal;
    public FileLocking(ITerminalInterface terminal, ICommandBuilder commandBuilder)
    {
        _terminal = terminal;
        _commandBuilder = commandBuilder;
    }

    public void LockFile()
    {
        /*
         * 0 save current branch
         * 1 switch file lock
         * 2 write lock to file
         * commit
         * 3 push
         * 4 switch back to saved branch 
         */

        string saveBranchCmd = _commandBuilder.GetCurrentBranch();
        string currentBranch = _terminal.ExecuteResultToString(saveBranchCmd);

        string switchFileLockingCmd = _commandBuilder.GetSwitch(GlobalRefs.lockingBranch);
        _terminal.Execute(switchFileLockingCmd);

        string writeLockCmd = _commandBuilder.GetEcho(GlobalRefs.filePaths.fileToLockNameOrPathLetsSee);
        _terminal.ExecuteResultToTxt(writeLockCmd, GlobalRefs.filePaths.lockedProtocolFile);

        string commitCmd = _commandBuilder.GetCommit(" . ");
        _terminal.Execute(commitCmd);

        string pushCmd = _commandBuilder.GetPush(" "); // Push the branch it is on, no name needed.
        _terminal.Execute(pushCmd);

        string switchBackCmd = _commandBuilder.GetSwitch(currentBranch);
        _terminal.Execute(switchBackCmd);
        
        LogSystem.WriteLog(new []
        {
            saveBranchCmd, switchFileLockingCmd, writeLockCmd, commitCmd,
            pushCmd, switchBackCmd
        });
    }
}