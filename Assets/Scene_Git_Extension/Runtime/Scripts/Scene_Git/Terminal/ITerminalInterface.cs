public interface ITerminalInterface
{
    /// <summary>
    /// File location specified in implementation.
    /// </summary>
    /// <param name="command"></param>
    public void ExecuteResultToTxt(string command);

    public string ExecuteResultToVar(string command);
}