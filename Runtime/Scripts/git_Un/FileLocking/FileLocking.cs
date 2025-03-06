public class FileLocking
{
    private ICommandBuilder _commandBuilder;
    private ITerminalInterface _terminal;
    public FileLocking(ITerminalInterface terminal, ICommandBuilder commandBuilder)
    {
        _terminal = terminal;
        _commandBuilder = commandBuilder;
    }

    public void LockFile(string fileToLock)
    {
        
    }
}