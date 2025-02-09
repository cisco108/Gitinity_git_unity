public interface ITerminalInterface
{
    /// <summary>
    /// File location specified in implementation.
    /// Same location also accessed when reading diff from IGitDiffReader. 
    /// </summary>
    /// <param name="command"></param>
    public void ExecuteResultToTxt(string command);

    /// <summary>
    /// Writes it to txt first, then extracts string.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public string ExecuteResultToVar(string command);
}